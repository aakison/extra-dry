using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExtraDry.Server;

/// <summary>
/// Provides business rule logic for Creating, Updating and Deleting objects from untrusted sources.
/// The untrusted source is usually an object deserialized from JSON from an API or MVC call.
/// The rules for overwriting, ignoring, or blocking changes are defined by applying the `RuleAttribute` to each property.
/// The `RuleEngine` should then be dependency injected into services where its methods will consistently apply business rules.
/// </summary>
public class RuleEngine {

    /// <summary>
    /// Creates a new RuleEngine, typically only called from the DI service.
    /// The IServiceProvider is used to further discover IEntityResolver objects for cases where the rule engine
    /// is attempting to link to an existing object.
    /// </summary>
    public RuleEngine(IServiceProvider serviceProvider)
    {
        services = serviceProvider;
    }

    /// <summary>
    /// When copying an object (during create or update) that allows for nested objects, the number
    /// of nesting that the system will stop at.  This will prevent recursion issues when two objects reference
    /// each other.  This can be increased if necessary for large trees of data.
    /// </summary>
    public int MaxRecursionDepth { get; set; } = 20;

    /// <summary>
    /// Given an potentially untrusted and unvalidated exemplar of an object, create a new copy of that object with business rules applied.
    /// Any validation issues or rule violations will throw an exception.
    /// </summary>
    public async Task<T> CreateAsync<T>(T exemplar)
    {
        if(exemplar == null) {
            throw new ArgumentNullException(nameof(exemplar));
        }
        var validator = new DataValidator();
        validator.ValidateObject(exemplar);
        validator.ThrowIfInvalid();
        var destination = Activator.CreateInstance<T>();
        await UpdatePropertiesAsync(exemplar, destination, MaxRecursionDepth, e => e.CreateAction);
        return destination;
    }

    /// <summary>
    /// Updates the `destination` with properties from `source`, while applying business logic rules in annotations.
    /// The source object will be validated using data annotations first, then properties will be copied across, where:
    ///   `JsonIgnore` properties will be skipped, these are typically empty as the source has likely been deserialized from the network;
    ///   `Rules(UpdateAction.BlockChanges)` annotations will throw an exception if a change is attempted;
    ///   `Rules(UpdateAction.Ignore)` annotations will not be copied;
    ///   `Rules(UpdateAction.IgnoreDefaults)` annotations will not be copied if the source property is `null` or `default`.
    /// </summary>
    public async Task UpdateAsync<T>(T source, T destination)
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
        await UpdatePropertiesAsync(source, destination, MaxRecursionDepth, e => e.UpdateAction);
    }

    /// <summary>
    /// Processes a delete of an item setting the appropriate fields.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    public DeleteResult TrySoftDelete(object item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        var items = new[] { item };
        return TrySoftDelete(items);
    }

    /// <summary>
    /// Processes deletes for multiple items setting the appropriate fields.
    /// </summary>
    /// <param name="items">The items to delete.</param>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep as standard service instance style for DI.")]
    public DeleteResult TrySoftDelete(object[] items)
    {
        return AttemptSoftDelete(items) ? DeleteResult.SoftDeleted : DeleteResult.NotDeleted;
    }

    /// <inheritdoc cref="DeleteAsync{T}(T, Func{Task}, Func{Task}?)" />
    public DeleteResult Delete<T>(T item, Action remove, Action commit)
    {
        // Bit hinky, map Actions to Func<Task> so that logic flow isn't repeated, then map back to non-async.
        var task = DeleteAsync(item, WrapAction(remove), WrapAction(commit));
        CompleteActionMasquardingAsFuncTask(task);
        return task.Result;
    }

    /// <inheritdoc cref="DeleteAsync{T}(T, Func{Task}, Func{Task}?)" />
    public async Task<DeleteResult> DeleteAsync<T>(T item, Action remove, Func<Task> commit)
    {
        return await DeleteAsync(item, WrapAction(remove), commit);
    }

    /// <inheritdoc cref="DeleteAsync{T}(T, Func{Task}, Func{Task}?)" />
    public async Task<DeleteResult> DeleteAsync<T>(T item, Func<Task> remove, Action commit)
    {
        return await DeleteAsync(item, remove, WrapAction(commit));
    }

    /// <summary>
    /// Given an item that has previously been deleted, attempts to undelete the item.
    /// Will check that item can be undeleted based on `DeleteRule` on entity and that the item
    /// is actually in a deleted state.
    /// </summary>
    /// <param name="item">The item to be undeleted</param>
    /// <returns>Status of the undelete action.  If no undelete value exists or this item is not in a deleted state, then will return NoUndeleted.</returns>
    /// <exception cref="DryException">If the `DeleteRule` attribute has invalid properties or values.</exception>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Orthogonal with other members and might need to be made instance level in the future.")]
    public UndeleteResult Undelete<T>(T item)
    {
        if(item == null) {
            throw new ArgumentNullException(nameof(item));
        }
        var type = typeof(T);
        var deleteRule = type.GetCustomAttribute<DeleteRuleAttribute>();
        if(deleteRule == null || deleteRule.CanUndelete == false) {
            return UndeleteResult.NotUndeleted;
        }
        var property = type.GetProperty(deleteRule.PropertyName, BindingFlags.Instance | BindingFlags.Public);
        if(property == null) {
            throw new DryException($"Can't complete undelete, property '{deleteRule.PropertyName}' does not exist as a public instance property of class '{type.Name}'.", "Can't undelete item, internal application issue.");
        }
        var value = property.GetValue(item);
        if(!AreEqual(deleteRule.DeleteValue, value)) {
            return UndeleteResult.NotUndeleted;
        }
        try {
            property.SetValue(item, deleteRule.UndeleteValue);
        }
        catch {
            throw new DryException($"Can't complete undelete, value provided is not convertable to type of property '{deleteRule.PropertyName}", "Can't undelete item, internal application issue.");
        }
        return UndeleteResult.Undeleted;
    }

    /// <summary>
    /// Processes a hard delete of an item if possible.
    /// If not possible, then soft-delete is performed instead.
    /// </summary>
    /// <param name="item">The item to delete, a soft-delete is attempted first.</param>
    /// <param name="remove">If item can't be soft-deleted, then the action that is executed for a hard-delete.</param>
    /// <param name="commit">If item can't be soft-deleted, then the optional action that is executed to commit hard-delete.</param>
    public async Task<DeleteResult> DeleteAsync<T>(T item, Func<Task> remove, Func<Task> commit)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        var result = DeleteResult.NotDeleted;

        if(TrySoftDelete(item) == DeleteResult.SoftDeleted) {
            await commit();
            result = DeleteResult.SoftDeleted;
        }

        if(await TryHardDeleteAsync(item, remove, commit) == DeleteResult.HardDeleted) {
            result = DeleteResult.HardDeleted;
        }

        return result;
    }

    /// <summary>
    /// Processes a hard delete for multiple items if possible.
    /// If not possible, then soft-delete is performed for all items instead.
    /// Uses the remove actions from <see cref="RegisterRemove{T}(Action{T})" /> and commit action from <see cref="RegisterCommit(Func{Task})" />
    /// </summary>
    /// <param name="items">The list of items to delete.</param>
    public async Task<DeleteResult> DeleteAsync(params object[] items)
    {
        var result = DeleteResult.NotDeleted;
        if(TrySoftDelete(items) == DeleteResult.SoftDeleted) {
            await CommitFunctor();
            result = DeleteResult.SoftDeleted;
        }

        if(await TryHardDeleteAsync(items) == DeleteResult.HardDeleted) {
            result = DeleteResult.HardDeleted;
        }

        return result;
    }

    /// <inheritdoc cref="TryHardDeleteAsync{T}(T, Func{Task}?, Func{Task}?)" />
    public async Task<DeleteResult> TryHardDeleteAsync<T>(T item, Action remove, Func<Task> commit)
    {
        return await TryHardDeleteAsync(item, WrapAction(remove), commit);
    }

    /// <inheritdoc cref="TryHardDeleteAsync{T}(T, Func{Task}?, Func{Task}?)" />
    public async Task<DeleteResult> TryHardDeleteAsync<T>(T item, Func<Task> remove, Action commit)
    {
        return await TryHardDeleteAsync(item, remove, WrapAction(commit));
    }

    /// <summary>
    /// Processes a hard delete of an item.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    /// <param name="remove">The action that is executed for a hard-delete.</param>
    /// <param name="commit">The action that is executed to commit hard-delete.</param>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep as standard service instance style for DI.")]
    public async Task<DeleteResult> TryHardDeleteAsync<T>(T item, Func<Task> remove, Func<Task> commit)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        if(remove == null) {
            throw new ArgumentNullException(nameof(item), "Must provide an action to prepare hard delete, e.g. remove from collection");
        }

        if(commit == null) {
            throw new ArgumentNullException(nameof(item), "Must provide an action to commit hard delete, e.g. save to database");
        }
        var result = DeleteResult.NotDeleted;

        try {
            await remove();
            await commit();
            result = DeleteResult.HardDeleted;
        }
        catch {
            //Do not throw exceptions on commit- Just return appropriate result.
        }
        return result;
    }

    /// <summary>
    /// Register the remove action to be used when hard-deleting an item.
    /// </summary>
    /// <param name="remove">The action that is executed for a hard-delete.</param>
    public void RegisterRemove<T>(Action<T> remove)
    {
        RemoveFunctors.Add(typeof(T), e => remove((T)e));
    }

    /// <summary>
    /// Register the commit action to be used when hard-deleting an item.
    /// </summary>
    /// <param name="commit">The action that is executed to commit hard-delete.</param>
    public void RegisterCommit(Func<Task> commit)
    {
        CommitFunctor = commit;
    }

    /// <inheritdoc cref="TryHardDeleteAsync(object[])" />
    public async Task<DeleteResult> TryHardDeleteAsync<T>(T item) where T : class
    {
        Action<object> remove;
        try {
            remove = RemoveFunctors[typeof(T)];
        }
        catch(KeyNotFoundException ex) {
            throw new DryException($"Could not find registered expression for removing Entity. Register Entity removal with the RegisterRemove method.", ex);
        }

        var result = DeleteResult.NotDeleted;

        try {
            remove(item);
            await CommitFunctor();
            result = DeleteResult.HardDeleted;
        }
        catch {
            //Do not throw exceptions on commit - Just return appropriate result.
        }
        return result;
    }

    /// <summary>
    /// Processes a Hard Delete for multiple items.
    /// Uses the remove actions from <see cref="RegisterRemove{T}(Action{T})"/> and commit action from <see cref="RegisterCommit(Func{Task})"/>.
    /// </summary>
    /// <param name="items">Items to delete</param>
    public async Task<DeleteResult> TryHardDeleteAsync(params object?[] items)
    {
        var result = DeleteResult.NotDeleted;

        //Check all RemoveFunctors are available
        try {
            foreach(var item in items.Where(e => e is not null)) {
                Action<object>? remove;

                //If item is an IEnumerable check its RemoveFunctor
                if(item is IEnumerable enumerableItem) {
                    remove = RemoveFunctors[enumerableItem.GetType().GetGenericArguments().First()];
                    if(remove == null) {
                        return result;
                    }
                }
                else {
                    remove = RemoveFunctors[item!.GetType()];
                    if(remove == null) {
                        return result;
                    }
                }
            }
        }
        catch(KeyNotFoundException ex) {
            throw new DryException($"Could not find registered expression for removing Entity. Register Entity removal with the RegisterRemove method.", ex);
        }

        //Perform the removes & commit
        try {
            foreach(var item in items.Where(e => e is not null)) {
                Action<object>? remove;

                if(item is IEnumerable enumerableItem) {
                    remove = RemoveFunctors[enumerableItem.GetType().GetGenericArguments().First()];
                    foreach(var eItem in enumerableItem) {
                        remove(eItem);
                    }
                }
                else {
                    remove = RemoveFunctors[item!.GetType()];
                    remove(item);
                }
            }
            await CommitFunctor();
            result = DeleteResult.HardDeleted;
        }
        catch {
            //Do not throw exceptions on commit - Just return appropriate result.
        }
        return result;
    }

    private async Task UpdatePropertiesAsync<T>(T source, T destination, int depth, Func<RulesAttribute, RuleAction> selector)
    {
        if(depth == 0) {
            throw new DryException("Recursion limit on update reached");
        }
        var type = typeof(T);
        var properties = type.GetProperties();
        var deleteRuleAttribute = type.GetCustomAttribute<DeleteRuleAttribute>();

        foreach(var property in properties) {
            var ignore = property.GetCustomAttribute<JsonIgnoreAttribute>();
            var rulesAttribute = property.GetCustomAttribute<RulesAttribute>();
            var action = EffectiveRule(property, ignore, selector, RuleAction.Allow, rulesAttribute);
            if(action == RuleAction.Ignore) {
                continue;
            }
            (var sourceValue, var destinationValue) = GetPropertyValues(property, source, destination);
            if(sourceValue == null && destinationValue == null) {
                // Common occurrence which we can short circuit all future logic for performance.
                // Also allows processing below to ignore this case.
            }
            else if(typeof(IList).IsAssignableFrom(property.PropertyType)) {
                var sourceList = sourceValue as IList;
                await ProcessCollectionUpdates(action, property, destination, sourceList);
            }
            else {
                await ProcessIndividualUpdate(action, property, destination, sourceValue, depth, ignore, selector, deleteRuleAttribute);
            }
        }
    }

    private static (object? sourceValue, object? destinationValue) GetPropertyValues<T>(PropertyInfo property, T source, T destination)
    {
        try {
            var sourceValue = property.GetValue(source);
            var destinationValue = property.GetValue(destination);
            return (sourceValue, destinationValue);
        }
        catch(TargetParameterCountException) {
            return (null, null);
        }
    }

    private static RuleAction EffectiveRule(PropertyInfo property, JsonIgnoreAttribute? ignore, Func<RulesAttribute, RuleAction> selector, RuleAction defaultType, RulesAttribute? rulesAttribute)
    {
        if(property.SetMethod == null) {
            return RuleAction.Ignore;
        }
        if(rulesAttribute != null) {
            return selector(rulesAttribute);
        }
        else if(ignore?.Condition == JsonIgnoreCondition.Always) {
            return RuleAction.Ignore;
        }
        else {
            return defaultType;
        }
    }

    private async Task ProcessIndividualUpdate<T>(RuleAction action, PropertyInfo property, T destination, object? value, int depth, JsonIgnoreAttribute? ignoreAttribute, Func<RulesAttribute, RuleAction> selector, DeleteRuleAttribute? deleteRuleAttribute)
    {
        // Check against null for object types and GetDefaultValue for boxed value types.
        if(action == RuleAction.IgnoreDefaults && (value == null || value.Equals(property.PropertyType.GetDefaultValue()))) {
            return;
        }
        (var resolved, var result) = await ResolveEntityValue(property.PropertyType, value);
        var destinationValue = property.GetValue(destination);
        if(action == RuleAction.Link && !resolved) {
            // Copy object reference
            property.SetValue(destination, result);
        }
        else if(!resolved && !property.PropertyType.IsValueType && property.PropertyType != typeof(string)) {
            // Recurse through child to copy values
            if(value != null && destinationValue == null) {
                destinationValue = Activator.CreateInstance(value.GetType());
                property.SetValue(destination, destinationValue);
            }
            if(destinationValue != null) {
                await UpdatePropertiesAsync((dynamic?)value, (dynamic)destinationValue, --depth, selector);
            }
        }
        else {
            bool same = AreEqual(result, destinationValue);
            if(action == RuleAction.Block && !same) {
                if(ignoreAttribute?.Condition == JsonIgnoreCondition.Always) {
                    return;
                }
                throw new DryException($"Invalid attempt to change property '{property.Name}'", $"Attempt to change read-only property '{property.Name}'");
            }

            // Do not allow property to be set to the value configured in SoftDeleteAttribute.
            if(!same && deleteRuleAttribute?.DeleteValue != null && deleteRuleAttribute.PropertyName == property.Name && AreEqual(value, deleteRuleAttribute.DeleteValue)) {
                throw new DryException($"Invalid attempt to change property '{property.Name}' to the DeleteValue", "Unable to update, an attempt was made to make the entity appear deleted. Check your values to prevent this or use the dedicated delete functionality if available.");
            }

            property.SetValue(destination, result);
        }
    }

    private static bool AreEqual(object? result, object? destinationValue) => (result == null && destinationValue == null) || (result?.Equals(destinationValue) ?? false);

    private async Task ProcessCollectionUpdates<T>(RuleAction action, PropertyInfo property, T destination, IList? sourceList)
    {
        var destinationValue = property.GetValue(destination);
        var destinationList = destinationValue as IList;
        if(destinationList == null && sourceList != null) {
            destinationList = Activator.CreateInstance(property.PropertyType) as IList;
            property.SetValue(destination, destinationList);
        }
        if(action == RuleAction.IgnoreDefaults && sourceList == null) {
            // Don't modify destination as source is in default state of null (note that an empty collection will change destination)
            return;
        }
        var sourceEntities = new List<object?>();
        if(sourceList != null) {
            var listItemType = property.PropertyType.GetGenericArguments()[0];
            foreach(var item in sourceList) {
                (var _, var value) = await ResolveEntityValue(listItemType, item);
                sourceEntities.Add(value);
            }
        }
        if(destinationList != null) {
            var destObjects = destinationList.Cast<object>();
            var toRemove = destObjects.Except(sourceEntities).ToList();
            var toAdd = sourceEntities.Except(destObjects).ToList();
            if(action == RuleAction.Block && (toRemove.Any() || toAdd.Any())) {
                throw new DryException($"Invalid attempt to change collection property {property.Name}", $"Attempt to change read-only collection property '{property.Name}'");
            }
            foreach(var item in toRemove) {
                destinationList.Remove(item); // Only null if null-to-null copy which is handled.
            }
            foreach(var item in toAdd) {
                destinationList.Add(item);
            }
        }
    }

    /// <summary>
    /// Given an object, especially one that has been deserialized, attempts to resolve a version of that object
    /// that might be a database entity.  Uses `IEntityResolver` class in DI to find potential replacement.
    /// This only works for objects, value types are always copies, so sourceValue is always returned.
    /// </summary>
    private async Task<(bool, object?)> ResolveEntityValue(Type type, object? sourceValue)
    {
        if(type.IsValueType || type == typeof(string)) {
            return (false, sourceValue);
        }
        var untypedEntityResolver = typeof(IEntityResolver<>);
        var typedEntityResolver = untypedEntityResolver.MakeGenericType(type);
        var resolver = services.GetService(typedEntityResolver);
        if(resolver == null) {
            return (false, sourceValue);
        }
        else {
            var method = typedEntityResolver.GetMethod("ResolveAsync");
            if(method == null) {
                throw new DryException($"Resolver '{type.Name}' object missing method ResolveAsync");
            }
            // Force not-null return as ResolveAsync above is not-null return.
            dynamic task = method.Invoke(resolver, new object?[] { sourceValue })!;
            var result = (await task) as object;
            return (true, result);
        }
    }

    private static Func<Task> WrapAction(Action action)
    {
        return () => {
            action();
            return Task.CompletedTask;
        };
    }

    private static void CompleteActionMasquardingAsFuncTask(Task task)
    {
        if(task.IsCompleted && task.IsFaulted) {
            throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Aggregate exception occurred but was missing details.");
        }
    }

    private static bool AttemptSoftDelete<T>(T item)
    {
        var type = typeof(T);
        var softDelete = type.GetCustomAttribute<DeleteRuleAttribute>();
        if(softDelete == null) {
            return false;
        }
        var property = type.GetProperty(softDelete.PropertyName, BindingFlags.Instance | BindingFlags.Public);
        if(property == null) {
            return false;
        }
        try {
            property.SetValue(item, softDelete.DeleteValue);
        }
        catch {
            return false;
        }
        return true;
    }

    private static bool AttemptSoftDelete(object[] items)
    {
        var data = items
            .Where(e => e != null)
            .Select(e => new SoftDeleteItem {
                Item = e,
                DeleteRuleAttribute = e.GetType().GetCustomAttribute<DeleteRuleAttribute>(),
                PropInfo = null
            })
            .ToList();

        //Check SoftDeleteRule attribute is set on all items - Fail all if any missing.
        if(data.Any(e => e.DeleteRuleAttribute == null)) {
            return false;
        }

        foreach(var item in data) {
            item.PropInfo = item.Item.GetType().GetProperty((string)item.DeleteRuleAttribute!.PropertyName, BindingFlags.Instance | BindingFlags.Public);
        }

        //SoftDeleteRuleAttribute set on Invalid Property
        if(data.Any(e => e.PropInfo == null)) {
            return false;
        }

        foreach(var item in data) {
            try {
                item.PropInfo!.SetValue((object)item.Item, (object?)item.DeleteRuleAttribute!.DeleteValue);
            }
            catch {
                throw new DryException($"Can't complete soft-delete, value provided is not convertable to type of property '{item.DeleteRuleAttribute!.PropertyName}", "Can't delete item, internal application issue.");
            }
        }

        return true;
    }

    private class SoftDeleteItem {
        public required object Item { get; set; }
        public DeleteRuleAttribute? DeleteRuleAttribute { get; set; }
        public PropertyInfo? PropInfo { get; set; }

    }

    private readonly IServiceProvider services;
    private Dictionary<Type, Action<object>> RemoveFunctors { get; set; } = new();
    private Func<Task> CommitFunctor { get; set; } = () => Task.CompletedTask;
}
