using DapperCodeFirstMappings;
using Sqliste.Core;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.SqlServer;
using Sqliste.Database.SqlServer.Configuration;
using Sqliste.Infrastructure;

namespace Sqliste.Server.Extensions;

public static class DapperExtensions
{
    public static void LoadMappings()
    {
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(SqlisteCoreReflexionStub).Assembly);
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(SqlisteInfrastructureReflexionStub).Assembly);
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(SqlisteDatabaseSqlServerReflexionStub).Assembly);
        DapperEntitiesMappingUtils.LoadMappingsFromAssembly(typeof(Program).Assembly);
    }
}