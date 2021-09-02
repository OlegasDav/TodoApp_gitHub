using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services
                .AddSqlClient()
                .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddSingleton<ITodoRepository, TodoRepository>();
        }

        private static IServiceCollection AddSqlClient(this IServiceCollection services)
        {
            var fluentConnectionStringBuilder = new FluentConnectionStringBuilder();

            var connectionString = fluentConnectionStringBuilder
                .AddServer("localhost")
                .AddPort(3306)
                .AddUserId("userOleg")
                .AddPassword("rootroot")
                .AddDatabase("todosapp")
                .BuildConnectionString(true);

            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }
    }
}
