using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ExtraDry.UploadTools
{
    public class FileService {

        private List<FileTypeDefinition> FileDefinitions;

        public FileService()
        {
            if(FileDefinitions == null) { LoadFileDefinitions(); }
        }

        private void LoadFileDefinitions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ExtraDry.UploadTools.FileDatabase.json";

            using(var stream = assembly.GetManifestResourceStream(resourceName)) {
                using(var reader = new StreamReader(stream)) {
                    var fileContent = reader.ReadToEnd();
                    FileDefinitions = JsonSerializer.Deserialize<List<FileTypeDefinition>>(fileContent);
                }
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
                return null;
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
}
