namespace Blazor.ExtraDry {
    public enum CreateAction {
        /// <summary>
        /// 'RuleEngine' will process the property based on the type.
        /// Value types and string will default to 'LinkExisting'.  Reference types will default to 'Create'.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Value will be ignored by the 'RuleEngine'.
        /// </summary>
        Ignore = 1,
        /// <summary>
        /// 
        /// </summary>
        IgnoreDefault = 2,
        /// <summary>
        /// Create a new copy of the object with business rules applied.
        /// </summary>
        Create = 3,
        /// <summary>
        /// Create a new copy of the object and it's descendants with business rules applied.
        /// </summary>
        CreateDescendants = 4,
        /// <summary>
        /// Retains the source value.
        /// </summary>
        LinkExisting = 5,
    }
}
