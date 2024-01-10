using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor;

/// <summary>
/// Extensions for FileValidator that only apply when running on a Blazor client.
/// </summary>
public static class FileValidatorExtensions
{

    /// <summary>
    /// Validates a file given the <see cref="IBrowserFile"/> from a web form.  If the file is 
    /// invalid, the list of invalid reasons is returned.  For multiple file uploads, call this 
    /// multiple times and retrieve the results fromt the Errors property.
    /// </summary>
    public static IEnumerable<ValidationResult> ValidateFile(this FileValidator source, IBrowserFile file)
    {
        return source.ValidateFile(file.Name, file.ContentType);
    }

}

