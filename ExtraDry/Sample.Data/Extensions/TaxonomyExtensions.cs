using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sample.Data {
    public static class TaxonomyExtensions {
        /// <summary>
        /// Moves a subtree from one point in the heirarchy to another.
        /// </summary>
        /// <remarks>
        /// This does not use entity tracking and does a bulk-update to the database.It will participate in enclosing transactions and care should be taken that the Ancestors and Descendants are not otherwise changed in the same transaction.
        /// </remarks>
        public static async Task MoveSubtree(this ITaxonomyEntity subtreeRootToMove, ITaxonomyEntity newParent, DbContext context)
        {
            // Check that the new parent is at the same strata level as the current parent.
            if(subtreeRootToMove.Strata != newParent.Strata + 1) {
                throw new ArgumentException("Moving the subtree cannot change it's strata level");
            }
            if(context.ChangeTracker.Entries().Any(e => e.State != EntityState.Unchanged)) {
                throw new ArgumentException("Moving the subtree needs to happen in isolation from all other changes.");
            }
            if(context.Database.CurrentTransaction == null) {
                throw new ArgumentException("This method must be called from within a transaction.");
            }

            // This should align with the name of the closure table due to EF convention. eg. Region -> RegionRegion, Location -> LocationLocation
            var closureTableName = $"[{subtreeRootToMove.GetType().Name}{subtreeRootToMove.GetType().Name}]";

            // Query based off the closure table move-subtree queries as found in the book SQL Antipatterns by Bill Karwin
            // First step deletes references from the super tree to items within the subtree, retaining the self-reference of the root node.
            await context.Database.ExecuteSqlRawAsync($@"DELETE FROM {closureTableName}
WHERE​ DescendantsId ​IN​ (​SELECT DescendantsId FROM​ {closureTableName} WHERE​ AncestorsId = {subtreeRootToMove.Id})
AND​ AncestorsId ​IN​ (​SELECT​ AncestorsId FROM​ {closureTableName} ​WHERE DescendantsId = {subtreeRootToMove.Id} ​AND​ AncestorsId != DescendantsId );");

            // Then we insert to the new location, adding links from the items in the subtree to the items in the new parent ancestor path.
            await context.Database.ExecuteSqlRawAsync($@"INSERT INTO {closureTableName} (AncestorsId, DescendantsId)
SELECT supertree.AncestorsId, subtree.DescendantsId
FROM {closureTableName} supertree
	CROSS JOIN {closureTableName} subtree
WHERE supertree.DescendantsId = {newParent.Id}
	and subtree.AncestorsId = {subtreeRootToMove.Id}");
        }
    }
}
