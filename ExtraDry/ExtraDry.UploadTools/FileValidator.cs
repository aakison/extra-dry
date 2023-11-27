using ExtraDry.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtraDry.UploadTools
{
    public class FileValidator
    {
        public bool Success { get; set; } = true;

        private DryException ErrorMessage { get; set; }

        public FileValidator(string filename, string mimeType, byte[] content, UploadTools tools)
        {
            try {
                tools.ValidateFile(filename, mimeType, content);
            }
            catch(DryException ex) {
                ErrorMessage = ex;
                Success = false;
            }
        }

        public void ThrowIfError()
        {
            throw ErrorMessage;
        }
    }
}
