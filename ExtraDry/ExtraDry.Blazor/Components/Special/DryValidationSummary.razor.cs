﻿using ExtraDry.Core.Models;
using System.Text.Json;

namespace ExtraDry.Blazor;

public partial class DryValidationSummary : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// If an exception was raised in the application, provides the details of the Exception.
    /// </summary>
    [Parameter]
    public Exception? Exception { get; set; }

    /// <summary>
    /// If ProblemDetails are available (per RFC7231) they are provided here. This will be the case
    /// with any RFC7231 APIs that return problem details on any 4xx response.
    /// </summary>
    [Parameter]
    public ProblemDetails? ProblemDetails { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    public List<string> GetValidationMessages()
    {
        if(!IsValidationError) {
            return [];
        }
        if(!ProblemDetails!.Extensions.TryGetValue("errors", out var errors)) {
            return [];
        }

        var alertMessages = new List<string>();
        if(errors is not JsonElement) {
            return [];
        }
        var messages = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errors.ToString() ?? string.Empty);
        if(messages == null || messages.Keys.Count == 0) {
            return [];
        }
        foreach(var group in messages.Keys) {
            foreach(var error in messages[group]) {
                alertMessages.Add(FormatGroupMessage(group, error));
            }
        }

        return alertMessages;
    }

    /// <summary>
    /// Formats the message to be displayed in the alert bar. Typically for validation messages the
    /// group is the property name and the error is the message, which redundantly contains the
    /// group name. Identify these situations and clean them up.
    /// </summary>
    private static string FormatGroupMessage(string group, string message)
    {
        if(message.Contains(group)) {
            // the group is the property name, remove it.
            message = FormatMessage(group, message);
        }
        return $"{DataConverter.CamelCaseToTitleCase(group)}: {message}";
    }

    /// <summary>
    /// Given a validation message that contains a property name, remove the property name. This
    /// allows messages that are shown in context to not appear redundant.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="message">The validation messages</param>
    /// <returns>The formatted message</returns>
    public static string FormatMessage(string propertyName, string message)
    {
        message = message.Replace($"The {propertyName} field ", "");
        message = message.Replace($"The field {propertyName} ", "");
        if(message.Length > 0) {
            message = $"{char.ToUpperInvariant(message[0])}{message[1..]}";
        }
        return message;
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "validation-summary", ErrorCss, CssClass);

    private string ErrorCss => IsValidationError ? $"status{ProblemDetails!.Status}" : "unexpected-error";

    private bool IsValidationError => ProblemDetails != null && ProblemDetails.Status == (int)HttpStatusCode.BadRequest;
}
