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
         * Read from Internal Json File
         * Allow for different json file to be prodided
         * Determine type based off file name
         * Determine type based off mime
         * Determine type based off byte[]
         * 
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

        internal List<FileTypeDefinition> GetFileType(string filename)
        {
            var extension = Path.GetExtension(filename).Trim('.');
            var options = FileDefinitions.Where(f => f.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
            return options.ToList();
        }

        internal List<FileTypeDefinition> GetFileType(byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}
