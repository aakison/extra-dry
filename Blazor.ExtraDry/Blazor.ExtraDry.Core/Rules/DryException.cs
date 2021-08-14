#nullable enable

using System;
using System.Runtime.Serialization;

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
        
        protected DryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// If available, an exception message that is suitable to show to users.
        /// E.g. certain validation exceptions can be shown, but null reference cannot.
        /// </summary>
        public string? UserMessage { get; set; }
    }
}
