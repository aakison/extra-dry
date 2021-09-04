#nullable enable

namespace ExtraDry.Blazor.Components.Internal {
    internal class OptionInfo {

        public OptionInfo(string key, string display, object? value)
        {
            Key = key;
            Display = display;
            Value = value;
        }

        public string Key { get; set; }

        public string Display { get; set; }

        public bool Selected { get; set; }

        public object? Value { get; set; }
    }
}
