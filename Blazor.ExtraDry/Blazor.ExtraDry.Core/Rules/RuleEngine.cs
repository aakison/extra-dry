#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Blazor.ExtraDry {
    public class RuleEngine {


        /// <summary>
        /// Given an potentially untrusted and unvalidated exemplar of an object, create a new copy of that object with business rules applied.
        /// Any validation issues or rule violations will throw an exception.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep as standard service instance style for DI.")]
        public T Create<T>(T exemplar)
        {
            if(exemplar == null) {
                throw new ArgumentNullException(nameof(exemplar));
            }
            var validator = new DataValidator();
            validator.ValidateObject(exemplar);
            validator.ThrowIfInvalid();
            var destination = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            foreach(var property in properties) {
                var rule = property.GetCustomAttribute<RulesAttribute>();
                var ignore = property.GetCustomAttribute<JsonIgnoreAttribute>();
                if(ignore != null && ignore.Condition == JsonIgnoreCondition.Always) {
                    continue;
                }
                var action = rule?.CreateAction ?? CreateAction.CreateNew;
                var sourceValue = property.GetValue(exemplar);
                var destinationValue = property.GetValue(destination);
                if(sourceValue?.Equals(destinationValue) ?? true) {
                    continue;
                }
                switch(action) {
                    // TODO: This logic requires more thought...
                    case CreateAction.CreateNew:
                    case CreateAction.MakeUnique:
                        continue;
                    default:
                        break;
                }
                property.SetValue(destination, sourceValue);
            }
            return destination;
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep as standard service instance style for DI.")]
        public void Update<T>(T source, T destination)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if(destination == null) {
                throw new ArgumentNullException(nameof(destination));
            }
            var validator = new DataValidator();
            validator.ValidateObject(source);
            validator.ThrowIfInvalid();
            var properties = typeof(T).GetProperties();
            foreach(var property in properties) {
                var rule = property.GetCustomAttribute<RulesAttribute>();
                var ignore = property.GetCustomAttribute<JsonIgnoreAttribute>();
                if(ignore != null && ignore.Condition == JsonIgnoreCondition.Always) {
                    continue;
                }
                var action = rule?.UpdateAction ?? UpdateAction.AllowChanges;
                var sourceValue = property.GetValue(source);
                var destinationValue = property.GetValue(destination);
                if(sourceValue?.Equals(destinationValue) ?? true) {
                    continue;
                }
                switch(action) {
                    case UpdateAction.IgnoreChanges:
                        continue;
                    case UpdateAction.BlockChanges:
                        if(!sourceValue.Equals(property.GetValue(destination))) {
                            throw new DryException($"Invalid attempt to change property {property.Name}", $"Attempt to change read-only property '{property.Name}'");
                        }
                        continue;
                    case UpdateAction.IgnoreDefaults:
                        if(sourceValue == default) {
                            continue;
                        }
                        break;
                    case UpdateAction.AllowChanges:
                    default:
                        break;
                }
                property.SetValue(destination, sourceValue);
            }
        }

        /// <summary>
        /// Processes a delete of an item setting the appropriate fields.
        /// If any fields are changed, then the item is soft-deleted and the method return true.
        /// Otherwise, it returns false and the object should be hard-deleted.
        /// </summary>
        /// <param name="item">The item to delete, a soft-delete is attempted first.</param>
        /// <param name="hardDeleteOption">If item can't be soft-deleted, then the action that is executed for a hard-delete.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep as standard service instance style for DI.")]
        public void Delete<T>(T item, Action? hardDeleteOption = null)
        {
            if(item == null) {
                throw new ArgumentNullException(nameof(item));
            }
            var properties = typeof(T).GetProperties();
            bool deleted = false;
            foreach(var property in properties) {
                var rule = property.GetCustomAttribute<RulesAttribute>();
                if(rule?.DeleteValue != null) {
                    deleted = true;
                    property.SetValue(item, rule.DeleteValue);
                }
            }
            if(!deleted) {
                if(hardDeleteOption == null) {
                    throw new InvalidOperationException("Item could not be soft-deleted and no hard delete action was provided.");
                }
                else {
                    hardDeleteOption();
                }
            }
        }
    }
}
