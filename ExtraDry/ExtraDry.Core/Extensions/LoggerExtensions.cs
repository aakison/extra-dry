using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Text;
using System;

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

        string Prefix() => $"  {++index}. ";
    }

    /// <summary>
    /// Logs the properties of the object and its children. Any properties that are marked with the
    /// <see cref="SecretAttribute" /> will be dispalyed as "*****" instead of their actual value.
    /// Validation is also checked and any errors are logged.
    /// </summary>
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

        logger.LogInformation("Resolved Configuration for '{Name}':\n  * {List}",
            displayOptions.Name, string.Join($"\n  * ", list));
        if(displayOptions.ValidationErrors.Count > 0) {
            var results = string.Join($"\n  * ", displayOptions.ValidationErrors);
            logger.LogWarning("Configuration Failed Validation:\n  * {Results}", results);
        }
    }

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
            Secrets.AddRange(secureProperties.Select(e => e.GetValue(target)?.ToString() ?? "").Where(e => e.Length > 1));
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
