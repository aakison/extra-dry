using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    [Serializable]
    public class DryException : Exception {
        public DryException() { }
        public DryException(string message) : base(message) { }
        public DryException(string message, Exception inner) : base(message, inner) { }

        public DryException(string message, string userMessage) : base(message)
        {
            UserMessage = userMessage;
        }
        protected DryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// If available, an exception message that is suitable to show to users.
        /// E.g. certain validation exceptions can be shown, but null reference cannot.
        /// </summary>
        public string UserMessage { get; set; }
    }
}
