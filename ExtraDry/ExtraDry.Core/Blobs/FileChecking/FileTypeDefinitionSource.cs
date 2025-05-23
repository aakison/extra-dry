﻿using System.Text.Json;

namespace ExtraDry.Core;

internal class FileTypeDefinitionSource(string fileDatabasePath = "")
{
    private static List<FileTypeDefinition> LoadFileDefinitionsFromAssembly(string fileDatabasePath)
    {
        string fileContent = "[]";
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var server = assemblies.FirstOrDefault(e => e.GetName().Name == "ExtraDry.Server");
        if(File.Exists(fileDatabasePath)) {
            fileContent = File.ReadAllText(fileDatabasePath);
        }
        else if(server != null) {
            var resourceName = "ExtraDry.Server.Blobs.FileDatabase.json";
            using var stream = server.GetManifestResourceStream(resourceName);
            if(stream != null) {
                using var reader = new StreamReader(stream);
                fileContent = reader.ReadToEnd();
            }
        }
        return JsonSerializer.Deserialize<List<FileTypeDefinition>>(fileContent) ?? [];
    }

    /// <summary>
    /// Add file definitions to the default database.
    /// </summary>
    internal void AddFileDefinitions(IEnumerable<FileTypeDefinition> fileTypeDefinitions)
    {
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
    internal IEnumerable<FileTypeDefinition> GetFileTypeFromContent(byte[]? content)
    {
        return content == null || content.Length == 0
            ? ([])
            : FileDefinitions.Where(m => IsMatch(content, m.MagicBytes));
    }

    private static bool IsMatch(byte[] content, List<MagicBytes> magic)
    {
        foreach(var m in magic) {
            if(IsMatch(content, m)) {
                return true;
            }
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

    private readonly List<FileTypeDefinition> FileDefinitions = LoadFileDefinitionsFromAssembly(fileDatabasePath);
}
