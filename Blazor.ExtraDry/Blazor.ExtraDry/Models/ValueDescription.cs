#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
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
