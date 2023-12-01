using System.Reflection;
using System.Text.Json;

namespace ExtraDry.Core;

internal class FileTypeDefinitionSource {

    public FileTypeDefinitionSource(string fileDatabasePath = "")
    {
        FileDefinitions = LoadFileDefinitionsFromAssembly(fileDatabasePath);
    }

    private static List<FileTypeDefinition> LoadFileDefinitionsFromAssembly(string fileDatabasePath)
    {
        try {
            string fileContent = "";
            if(File.Exists(fileDatabasePath)) {
                fileContent = File.ReadAllText(fileDatabasePath);
            }
            else {
                var assembly = Assembly.LoadFrom("ExtraDry.Server");
                var resourceName = "ExtraDry.Server.FileDatabase.json";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                fileContent = reader.ReadToEnd();
            }
            return JsonSerializer.Deserialize<List<FileTypeDefinition>>(fileContent) ?? new List<FileTypeDefinition>();
        }
        catch {
            // Swallow exceptions until we better determine how to load from file.
            return new List<FileTypeDefinition>();
        }
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
    internal IEnumerable<FileTypeDefinition> GetFileTypeFromFilename(string filename)
    {
        var extension = Path.GetExtension(filename).TrimStart('.');
        return FileDefinitions.Where(f => f.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Retrieves filetype definitions that match the provided mimetype
    /// </summary>
    internal IEnumerable<FileTypeDefinition> GetFileTypeFromMime(string mime)
    {
        return FileDefinitions.Where(f => f.MimeTypes.Contains(mime, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Retrieves filetype definitions where the magic bytes are found in the content
    /// </summary>
    internal IEnumerable<FileTypeDefinition> GetFileTypeFromBytes(byte[]? content)
    {
        if(content == null || content.Length == 0) {
            return Array.Empty<FileTypeDefinition>();
        }
        return FileDefinitions.Where(m => IsMatch(content, m.MagicBytes));
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

    private readonly List<FileTypeDefinition> FileDefinitions;

}
