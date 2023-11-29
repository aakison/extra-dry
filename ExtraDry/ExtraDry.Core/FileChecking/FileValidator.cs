namespace ExtraDry.Core;

/// <summary>
/// Provides validation services for files that are uploaded from Blazor clients to MVC servers.
/// The validation is partially performed client-side and a complete validation is performed 
/// server-side.  Configuration of the file validation rules are done using 
/// <see cref="FileValidationOptions"/> and the <see cref="FileValidationService"/>.  Use the 
/// <see cref="ServiceCollectionExtensions.AddFileValidation(Microsoft.Extensions.DependencyInjection.IServiceCollection, Action{ExtraDry.Core.FileValidationOptions}?)"/>
/// extension method register and configure the FileValidator.
/// </summary>
public class FileValidator
{
    /// <inheritdoc cref="FileValidator" />
    public FileValidator(FileValidationService fileValidationService)
    {
        validator = fileValidationService;
    }

    public void ValidateFile(string filename, string mimeType, byte[] content)
    {
        try {
            validator.ValidateFile(filename, mimeType, content);
        }
        catch(DryException ex) {
            ValidationError = ex;
            CanUpload = false;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the file that was put into this checker can be uploaded
    /// </summary>
    public bool CanUpload { get; private set; } = true;

    private DryException? ValidationError { get; set; }


    /// <summary>
    /// If the file that was checked was unable to be uploaded, this will throw the first validation exception that was encountered while checking
    /// </summary>
    public void ThrowIfError()
    {
        if(!CanUpload) {
            throw ValidationError ?? new DryException("Unknown file processing exception");
        }
    }

    private readonly FileValidationService validator;
}
