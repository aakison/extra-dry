using System.Collections.ObjectModel;

namespace ExtraDry.Core;

/// <summary>
/// Provides validation services for files that are uploaded from Blazor clients to MVC servers.
/// The validation is partially performed client-side and a complete validation is performed 
/// server-side.  Configuration of the file validation rules are done using 
/// <see cref="FileValidationOptions"/> and the <see cref="FileValidationService"/>.  Use the 
/// <see cref="ServiceCollectionExtensions.AddFileValidation"/>
/// extension method register and configure the FileValidator.
/// </summary>
public class FileValidator
{
    /// <inheritdoc cref="FileValidator" />
    public FileValidator(FileValidationService fileValidationService)
    {
        validator = fileValidationService;
    }

    /// <summary>
    /// Validates a file given the filename, mime type, and content.  If the file is invalid, the
    /// list of invalid reasons is returned.  For multiple file uploads, call this multiple times 
    /// and retrieve the results fromt the Errors property.
    /// </summary>
    public IEnumerable<ValidationResult> ValidateFile(string filename, string mimeType, byte[]? content = null)
    {
        var errors = validator.ValidateFile(filename, mimeType, content);
        ValidationErrors.AddRange(errors);
        return errors;
    }

    /// <summary>
    /// Validates a file referenced by a <see cref="IBlob"/>.  If the file is invalid, the list of 
    /// invalid reasons is returned.  For multiple file uploads, call this multiple times and 
    /// retrieve the results fromt the Errors property.
    /// </summary>
    public IEnumerable<ValidationResult> ValidateFile(IBlob blob)
    {
        return ValidateFile(blob.Slug, blob.MimeType, blob.Content);
    }

    /// <summary>
    /// Gets a value indicating whether the file (or files) that was validated with 
    /// <see cref="ValidateFile(string, string, byte[])" /> is free of validation errors.
    /// </summary>
    public bool IsValid => ValidationErrors.Count == 0;

    /// <summary>
    /// A list of validation errors that were encountered while validating the file or files.
    /// </summary>
    public ReadOnlyCollection<ValidationResult> Errors {
        get {
            return ValidationErrors.AsReadOnly();
        }
    }

    /// <summary>
    /// If the file or files that were validated had any validation errors, then throw a validation exception.
    /// </summary>
    public void ThrowIfInvalid()
    {
        if(!IsValid) {
            throw new ValidationException(string.Join(", ", ValidationErrors.Select(e => e.ErrorMessage)));
        }
    }

    [Obsolete("Use ThrowIfInvalid instead.")]
    public void ThrowIfNotValid() => ThrowIfInvalid();

    /// <summary>
    /// Clears the validator of errors so that it can be reused.
    /// </summary>
    public void Clear()
    {
        ValidationErrors.Clear();
    }

    private List<ValidationResult> ValidationErrors { get; } = new();

    private readonly FileValidationService validator;

}
