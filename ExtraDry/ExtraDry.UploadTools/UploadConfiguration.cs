using System;
using System.Collections.Generic;
using System.Text;

namespace ExtraDry.UploadTools {
    public class UploadConfiguration {
        public List<string> ExtensionWhitelist { get; set; } = new List<string>();
        
        // TODO - A way to define a file definition library
    }
}
