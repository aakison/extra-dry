namespace ExtraDry.Core {

    /// <summary>
    /// Actions that are used by rules to determine how creates and updates should be processed.
    /// </summary>
    public enum RuleAction {

        /// <summary>
        /// Changes to values will always be allowed by `RuleEngine`.
        /// </summary>
        Allow,

        /// <summary>
        /// Once created, no changes are processed by `RuleEngine`.
        /// </summary>
        Ignore,

        /// <summary>
        /// Once created, default source values will not change existing value processed by `RuleEngine`.
        /// </summary>
        IgnoreDefaults,

        /// <summary>
        /// Once created, attempts to change will cause an exception in the `RuleEngine`.
        /// This should be set on any property that a malicious actor would target to attack your system.
        /// </summary>
        Block,

        /// <summary>
        /// In the future we may have a need to block the Allow from recursing through a tree of children.
        /// Can't find a reason yet, but when we do this is how it will be implemented.
        /// </summary>
        // AllowOneLevel,
    }
}
