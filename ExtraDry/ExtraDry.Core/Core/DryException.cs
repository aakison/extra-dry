#nullable enable

using System;

namespace ExtraDry.Core {

    [Serializable]
    public sealed class DryException : Exception {
        
        public DryException() { }
        
        public DryException(string message) : base(message) { }
        
        public DryException(string message, Exception inner) : base(message, inner) { }

        public DryException(string message, string userMessage) : base(message)
        {
            UserMessage = userMessage;
        }
        
        /// <summary>
        /// If available, an exception message that is suitable to show to users.
        /// E.g. certain validation exceptions can be shown, but null reference cannot.
        /// </summary>
        public string? UserMessage { get; set; }
    }
}
