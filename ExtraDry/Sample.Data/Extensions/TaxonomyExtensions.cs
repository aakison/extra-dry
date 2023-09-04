using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.Extensions {
    public static class TaxonomyExtensions {
        public static async Task MoveSubtree(this DbContext context, ITaxonomyEntity subtreeRootToMove, ITaxonomyEntity newParent)
        {

        }

        public static async Task DeleteSubtree(this DbContext context, ITaxonomyEntity subtreeRoot)
        {
            // might be needed?
        }
    }
}
