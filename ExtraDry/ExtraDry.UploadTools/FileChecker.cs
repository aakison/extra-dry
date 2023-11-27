using ExtraDry.Core;

namespace ExtraDry.UploadTools
{
    public class FileChecker
    {
        /// <summary>
        /// Gets a value indicating whether the file that was put into this checker can be uploaded
        /// </summary>
        public bool CanUpload { get; } = true;

        private DryException ValidationError { get; set; }

        /// <summary>
        /// Creates a new file validator. At the time of creation, the validation is run
        /// </summary>
        public FileChecker(string filename, string mimeType, byte[] content, UploadService tools)
        {
            try {
                tools.ValidateFile(filename, mimeType, content);
            }
            catch(DryException ex) {
                ValidationError = ex;
                CanUpload = false;
            }
        }

        /// <summary>
        /// If the file that was checked was unable to be uploaded, this will throw the first validation exception that was encountered while checking
        /// </summary>
        public void ThrowIfError()
        {
            if(!CanUpload) {
                throw ValidationError;
            }
        }
    }
}
