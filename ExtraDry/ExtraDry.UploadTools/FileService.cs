using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExtraDry.UploadTools {
    internal class FileService {
        /* TODO -
         * Read from Internal Json File ✔️
         * Allow for different json file to be provided
         * Determine type based off file name ✔️
         * Determine type based off mime ✔️
         * Determine type based off byte[] ✔️
         * 
         * Out Of Scope - 
         * Reading Files
         * 
         */

        private static List<FileTypeDefinition> FileDefinitions;

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

        internal List<FileTypeDefinition> GetFileTypeFromFilename(string filename)
        {
            var extension = Path.GetExtension(filename).Trim('.');
            var options = FileDefinitions.Where(f => f.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
            return options.ToList();
        }

        internal List<FileTypeDefinition> GetFileTypeFromMime(string mime)
        {
            var options = FileDefinitions.Where(f => f.MimeTypes.Contains(mime, StringComparer.OrdinalIgnoreCase));
            return options.ToList();
        }

        internal bool MatchesMagicBytesOf(byte[] content, List<FileTypeDefinition> fileTypes)
        {
            if(content == null || content.Length < 1) {
                return false;
            }

            return fileTypes.Where(m => IsMatch(content, m.MagicBytes)).Any();
        }

        internal List<FileTypeDefinition> GetFileTypeFromBytes(byte[] content)
        {
            if(content == null || content.Length < 1) {
                return null;
            }

            return FileDefinitions.Where(m => IsMatch(content, m.MagicBytes)).ToList();
        }

        private bool IsMatch(byte[] content, List<MagicBytes> magic)
        {
            foreach(var m in magic) {
                if(IsMatch(content, m)){ return true; }
            }
            return false;
        }

        private bool IsMatch(byte[] content, MagicBytes magic)
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
