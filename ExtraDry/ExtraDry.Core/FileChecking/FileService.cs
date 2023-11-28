using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ExtraDry.Core;

public class FileService {

    private readonly List<FileTypeDefinition> FileDefinitions;

    public FileService(string fileDatabasePath)
    {
        FileDefinitions = LoadFileDefinitionsFromAssembly(fileDatabasePath);
    }


    private List<FileTypeDefinition> LoadFileDefinitionsFromAssembly(string fileDatabasePath)
    {
        if(!File.Exists(fileDatabasePath)) {
            throw new ArgumentException($"The file {fileDatabasePath} could not be found");
        }

        var fileContent = File.ReadAllText(fileDatabasePath);
        return JsonSerializer.Deserialize<List<FileTypeDefinition>>(fileContent) ?? new List<FileTypeDefinition>();
    }

    /// <summary>
    /// Add file definitions to the default database.
    /// </summary>
    internal void AddFileDefinitions(List<FileTypeDefinition> fileTypeDefinitions){
        FileDefinitions.AddRange(fileTypeDefinitions);
    }

    /// <summary>
    /// Retrieves filetype definitions that match the provided filename
    /// </summary>
    internal List<FileTypeDefinition> GetFileTypeFromFilename(string filename)
    {
        var extension = Path.GetExtension(filename).Trim('.');
        var options = FileDefinitions.Where(f => f.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
        return options.ToList();
    }

    /// <summary>
    /// Retrieves filetype definitions that match the provided mimetype
    /// </summary>
    internal List<FileTypeDefinition> GetFileTypeFromMime(string mime)
    {
        var options = FileDefinitions.Where(f => f.MimeTypes.Contains(mime, StringComparer.OrdinalIgnoreCase));
        return options.ToList();
    }

    /// <summary>
    /// Retrieves filetype definitions where the magic bytes are found in the content
    /// </summary>
    internal List<FileTypeDefinition> GetFileTypeFromBytes(byte[] content)
    {
        if(content == null || content.Length < 1) {
            return new List<FileTypeDefinition>();
        }

        return FileDefinitions.Where(m => IsMatch(content, m.MagicBytes)).ToList();
    }

    private static bool IsMatch(byte[] content, List<MagicBytes> magic)
    {
        foreach(var m in magic) {
            if(IsMatch(content, m)){ return true; }
        }
        return false;
    }

    private static bool IsMatch(byte[] content, MagicBytes magic)
    {
        if(magic == null || content == null || content.Length == 0) { return false; }

        var isMatch = content.Skip(magic.Offset).Take(magic.ValueAsByte.Length).SequenceEqual(magic.ValueAsByte);
        if(isMatch) {
            return true;
        }
        return false;
    }
}
