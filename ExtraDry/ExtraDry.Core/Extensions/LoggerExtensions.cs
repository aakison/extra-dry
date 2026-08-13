using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ExtraDry.Core;

/// <summary>
/// Extensions to the logger that displays all the properties of an object and its children. Any
/// properties that are marked with the <see cref="SecretAttribute" /> will be displayed as "*****"
/// instead of being displayed.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Logs the list of sources that were used to configure the application. This is useful for
    /// determining if the correct configuration files were loaded and in the correct order.
    /// </summary>
    public static void LogSources(this ILogger logger, ConfigurationManager configuration)
    {
        var sb = new StringBuilder();
        int index = 0;
        foreach(var source in configuration.Sources) {
            if(source is JsonConfigurationSource jsonSource) {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}Json File: {jsonSource.Path}");
            }
            else if(source is MemoryConfigurationSource memSource) {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}Memory Source");
                if(memSource.InitialData == null) {
                    sb.AppendLine("    * (empty)");
                }
                else {
                    foreach(var entry in memSource.InitialData) {
                        sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}{entry.Key} = {entry.Value}");
                    }
                }
            }
            else if(source is EnvironmentVariablesConfigurationSource) {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}Environment Variables");
            }
            else if(source is ChainedConfigurationSource) {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}Chained Source");
            }
            else {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Prefix()}Misc Source: {source.ToString()}");
            }
        }
        logger.LogInformation("Configuration sources:\n{Sources}", sb.ToString().TrimEnd());
        var time = DateTime.UtcNow.ToString("O");
        File.WriteAllTextAsync(logfile, $"Configuration Loaded: {time}\n\nConfiguration sources:\n{sb.ToString().TrimEnd()}\n\n");

        string Prefix() => $"  {++index}. ";
    }

    /// <summary>
    /// Logs the properties of the object and its children. Any properties that are marked with the
    /// <see cref="SecretAttribute" /> will be dispalyed as "*****" instead of their actual value.
    /// Validation is also checked and any errors are logged.
    /// </summary>
    /// <remarks>
    /// This is logged as a single structured log entry: the console (or any text-based sink) sees
    /// one readable, bulleted, multi-line message, while structured sinks (e.g. Application Insights
    /// via OpenTelemetry) receive each property as its own field, so they can be queried
    /// individually (e.g. filtering/grouping by "SqlServer:Server") instead of one opaque blob.
    /// </remarks>
    /// <param name="logger">The ILogger that the properties are logged to.</param>
    /// <param name="target">The object whose properties are logged.</param>
    public static void LogProperties(this ILogger logger, object? target = null)
    {
        if(target == null) {
            return;
        }
        var displayOptions = new DisplayOptions { Name = target.GetType().Name };
        displayOptions.ReadAndExpand(target);
        var list = displayOptions.Properties.Select(e => $"{e.Key}: {e.Value}");

        // No indentation is baked in here; the console formatter applies consistent indentation
        // to every line of a multi-line message, so bullets are indented one level below the
        // header regardless of the sink's own base indent.
        var message = $"Resolved Configuration for '{displayOptions.Name}':\n* {string.Join("\n* ", list)}";
        var state = new PropertyLogState(displayOptions.Name, message, displayOptions.Properties);
        logger.Log(LogLevel.Information, default, state, null, static (s, _) => s.ToString());

        var bullets = "* " + string.Join("\n* ", list);
        File.AppendAllText(logfile, $"\nResolved Configuration for '{displayOptions.Name}':\n{bullets}\n");
        if(displayOptions.ValidationErrors.Count > 0) {
            var results = string.Join($"\n  * ", displayOptions.ValidationErrors);
            logger.LogWarning("Configuration Failed Validation:\n  * {Results}", results);
        }
    }

    /// <summary>
    /// A structured log state that exposes each resolved configuration property as its own
    /// named field (via <see cref="IReadOnlyList{T}" /> of key/value pairs, the same mechanism
    /// <see cref="ILogger" /> message templates use). This lets structured logging sinks (e.g.
    /// OpenTelemetry/Application Insights) capture individual properties, while
    /// <see cref="ToString" /> still supplies one composed, human-readable message for
    /// text-based sinks like the console.
    /// </summary>
    private sealed class PropertyLogState : IReadOnlyList<KeyValuePair<string, object?>>
    {
        private readonly List<KeyValuePair<string, object?>> items;

        private readonly string message;

        public PropertyLogState(string name, string message, IReadOnlyDictionary<string, string> properties)
        {
            this.message = message;
            items = [.. properties.Select(p => new KeyValuePair<string, object?>(p.Key, p.Value)),
                new("Name", name),
                new("{OriginalFormat}", message)];
        }

        public int Count => items.Count;

        public KeyValuePair<string, object?> this[int index] => items[index];

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => message;
    }

    private const string logfile = "./appsettings.log";

    private class DisplayOptions
    {
        public string Name { get; set; } = "";

        public Dictionary<string, string> Properties { get; } = [];

        public List<string> Secrets { get; } = [];

        public List<string> ValidationErrors { get; } = [];

        public void ReadAndExpand(object target)
        {
            Properties.Clear();
            Secrets.Clear();
            LoadAll("", target);
            RemoveSecrets();
        }

        private void RemoveSecrets()
        {
            foreach(var property in Properties) {
                var display = property.Value;
                foreach(var secret in Secrets) {
                    display = display.Replace(secret, "*****");
                }
                Properties[property.Key] = display;
            }
        }

        private void LoadAll(string prefix, object target)
        {
            var type = target.GetType();
            var properties = type.GetProperties();
            var nestedProperties = properties.Where(e => e.PropertyType.IsClass
                && e.PropertyType.Name.Contains("Options"));
            foreach(var property in properties.Except(nestedProperties)) {
                if(property.GetCustomAttribute<JsonIgnoreAttribute>() != null) {
                    continue;
                }
                Properties.Add($"{prefix}{property.Name}", property.GetValue(target)?.ToString() ?? "<null>");
            }
            var secureProperties = properties.Where(e => e.GetCustomAttribute<SecretAttribute>() != null);
            Secrets.AddRange(secureProperties.Select(e => e.GetValue(target)?.ToString() ?? "")
                .Where(e => e.Length > 1 && !(e.StartsWith('{') && e.EndsWith('}'))));
            var validator = new DataValidator();
            validator.ValidateObject(target);
            foreach(var result in validator.Errors) {
                ValidationErrors.AddRange(validator.Errors.Select(e => ValidationMessage(prefix, e)));
            }
            foreach(var nested in nestedProperties) {
                var value = nested.GetValue(target);
                if(value is not null) {
                    LoadAll($"{prefix}{nested.Name}:", value);
                }
            }
        }

        private static string ValidationMessage(string prefix, ValidationResult result)
        {
            var members = string.Join(", ", result.MemberNames);
            return $"{prefix}{members}: {result.ErrorMessage}";
        }
    }
}
