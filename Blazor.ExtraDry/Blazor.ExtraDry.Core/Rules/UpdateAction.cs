using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.ExtraDry {
    public enum UpdateAction {
        /// <summary>
        /// Changes to values will always be allowed by `RuleEngine`.
        /// </summary>
        AllowChanges,

        /// <summary>
        /// Once created, no changes are procesed by `RuleEngine`.
        /// </summary>
        Ignore,

        /// <summary>
        /// Once created, default source values will not change existing value procesed by `RuleEngine`.
        /// </summary>
        IgnoreDefaults,

        /// <summary>
        /// Once created, attempts to change will cause an exception in the `RuleEngine`.
        /// </summary>
        BlockChanges,
    }
}
