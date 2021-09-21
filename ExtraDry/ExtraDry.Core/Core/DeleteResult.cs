namespace ExtraDry.Core {
    
    /// <summary>
    /// When a `RuleEngine` delete method is called, indicates the result of the action.
    /// </summary>
    public enum DeleteResult {

        /// <summary>
        /// The item was not deleted, this is an error condition and is not expected.
        /// </summary>
        NotDeleted,

        /// <summary>
        /// The item was soft-deleted.  One or more properties had a `DeleteValue` specified in the `RuleAttribute`.
        /// Lambda expressions for `prepare` and `commit` actions were _not_ executed.
        /// </summary>
        SoftDeleted,

        /// <summary>
        /// The item was hard-deleted.  
        /// Either a hard delete was requested or no properties had a `DeleteValue` specified in the `RuleAttribute`.
        /// Lambda expressions for `prepare` and `commit` actions were executed.
        /// </summary>
        HardDeleted,

    }
}
