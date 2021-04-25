using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blazor.ExtraDry {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RulesAttribute : ValidationAttribute {

        public RulesAttribute() : base("Not all rules satisfied.")
        {

        }

        public RulesAttribute(UpdateAction updateAction)
        {
            UpdateAction = updateAction;
        }

        public CreateAction CreateAction { get; set; }

        public UpdateAction UpdateAction { get; } = UpdateAction.AllowChanges;

        /// <summary>
        /// If set, provides a value that is applied to this property during a deletion.
        /// The `RuleEngine.Delete` method will return `true` if any properties have a DeleteValue.
        /// This should be interpreted as a soft-delete.
        /// </summary>
        public object DeleteValue { get; set; }



        public override bool IsValid(object value) => true;

    }

}
