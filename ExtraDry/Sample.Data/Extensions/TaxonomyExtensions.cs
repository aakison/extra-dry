using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Sample.Data {
    public static class TaxonomyExtensions {
        /// <summary>
        /// Moves a subtree from one point in the heirarchy to another.
        /// </summary>
        public static async Task MoveSubtree(this ITaxonomyEntity subtreeRootToMove, ITaxonomyEntity newParent, DbContext context)
        {
            if(subtreeRootToMove.Strata != newParent.Strata + 1) {
                throw new ArgumentException("Moving the subtree cannot change it's strata level");
            }

            // This should align with the name of the closure table. eg. Region -> RegionRegion, Location -> LocationLocation
            var closureTableName = $"[{subtreeRootToMove.GetType().Name}{subtreeRootToMove.GetType().Name}]";

            // Query based off the closure table move-subtree queries as found in the book SQL Antipatterns by Bill Karwin
            await context.Database.ExecuteSqlRawAsync($@"DELETE FROM {closureTableName}
WHERE​ DescendantsId ​IN​ (​SELECT DescendantsId FROM​ {closureTableName} WHERE​ AncestorsId = {subtreeRootToMove.Id})
AND​ AncestorsId ​IN​ (​SELECT​ AncestorsId FROM​ {closureTableName} ​WHERE DescendantsId = {subtreeRootToMove.Id} ​AND​ AncestorsId != DescendantsId );");

            await context.Database.ExecuteSqlRawAsync($@"INSERT INTO {closureTableName} (AncestorsId, DescendantsId)
SELECT supertree.AncestorsId, subtree.DescendantsId
FROM {closureTableName} supertree
	CROSS JOIN {closureTableName} subtree
WHERE supertree.DescendantsId = {newParent.Id}
	and subtree.AncestorsId = {subtreeRootToMove.Id}");
        }
    }
}
