using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ExtraDry.Server;

/// <summary>
/// When EF generates a comparison with a hierarchyid, it casts the value to a bit, which is not 
/// performant when run on SQL Server (several orders of magnitude!).  This interceptor removes
/// the Cast and replaces with an integer.  Only tagged queries are updated, use 
/// `IQueryable.TagWith(BrokenHierarchyIdCast.Tag)` to apply.  This is automatically applied when 
/// using the QueryWith(...) extension methods.
/// </summary>
/// <remarks>
/// This is a temporary fix until EF Core or SQL Server fixes this.
/// See https://github.com/dotnet/efcore/issues/27150
/// </remarks>
public class ImproveHierarchyQueryPerformance : DbCommandInterceptor {
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        ReplaceCast(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        ReplaceCast(command);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void ReplaceCast(DbCommand command)
    {
        if(command.CommandText.StartsWith($"-- {Tag}", StringComparison.Ordinal)) {
            command.CommandText = command.CommandText.Replace("CAST(1 AS bit)", "1");
        }
    }

    public const string Tag = "Remove Broken Cast";
}
