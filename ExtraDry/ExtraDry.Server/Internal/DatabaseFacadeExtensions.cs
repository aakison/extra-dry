using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExtraDry.Server.Internal;

internal static class DatabaseFacadeExtensions {

    // Not part of EF any more, need to hack it.  
    public static async Task<int> ExecuteScalerAsync(this DatabaseFacade facade, string sql)
    {
        using var cmd = facade.GetDbConnection().CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = System.Data.CommandType.Text;
        if(cmd.Connection!.State != System.Data.ConnectionState.Open) {
            cmd.Connection.Open();
        }
        var val = await cmd.ExecuteScalarAsync();
        return (val as int?) ?? -1;
    }

}
