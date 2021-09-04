#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExtraDry.Blazor {
    public class ValueDescription {

        public ValueDescription(object key, MemberInfo memberInfo)
        {
            Key = key;

            var display = memberInfo.GetCustomAttribute<DisplayAttribute>();
            Display = display?.Name ?? memberInfo.Name; // TODO: Format display name with global title case converter.


        }

        public object Key { get; set; }

        public string? Display { get; set; }

        public string? Image { get; set; }


    }
}
