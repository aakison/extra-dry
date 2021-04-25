using Blazor.ExtraDry.Models;
using System;

namespace Blazor.ExtraDry {

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute {

        public CommandAttribute()
        {
        }

        public CommandAttribute(CommandContext context)
        {
            Context = context;
        }

        public CommandContext Context { get; } = CommandContext.Alternate;

        public string Name { get; set; }

        /// <summary>
        /// The name of an icon to be applied to the command.
        /// This is added as class information to an 'i' tag with the command.
        /// </summary>
        public string Icon { get; set; }

        public int Order { get; set; }

        public CommandCollapse Collapse { get; set; }
    }

    public enum CommandCollapse {

        Never,

        Always,

        IconThenEllipses,

        StraightToEllipses,

    }


}
