using Microsoft.EntityFrameworkCore;
using System;

namespace Sample.Data {
    public static class TaxonomyExtensions {
        /// <summary>
        /// Moves a subtree from on point in the heirarchy to another.
        /// </summary>
        public static async Task MoveSubtree<T>(this DbContext context, int subtreeRootToMove, int newParent)
        {
            // This should align with the name of the closure table. eg. Region -> RegionRegion, Location -> LocationLocation
            var closureTableName = $"[{typeof(T).Name}{typeof(T).Name}]";

            // Query based off the closure table move-subtree queries as found in the book SQL Antipatterns by Bill Karwin
            await context.Database.ExecuteSqlRawAsync($@"DELETE FROM {closureTableName}
WHERE​ DescendantsId ​IN​ (​SELECT DescendantsId FROM​ {closureTableName} WHERE​ AncestorsId = {subtreeRootToMove})
AND​ AncestorsId ​IN​ (​SELECT​ AncestorsId FROM​ {closureTableName} ​WHERE DescendantsId = {subtreeRootToMove} ​AND​ AncestorsId != DescendantsId );");

            await context.Database.ExecuteSqlRawAsync($@"INSERT INTO {closureTableName} (AncestorsId, DescendantsId)
SELECT supertree.AncestorsId, subtree.DescendantsId
FROM {closureTableName} supertree
	CROSS JOIN {closureTableName} subtree
WHERE supertree.DescendantsId = {newParent}
	and subtree.AncestorsId = {subtreeRootToMove}");
        }
    }
}
