using Contracts.Enums;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            //SqlMapper.AddTypeHandler(new MySqlDifficultyTypeHandler());
            //SqlMapper.RemoveTypeMap(typeof(Difficulty));
            //SqlMapper.RemoveTypeMap(typeof(Difficulty?));

            return services
                .AddSqlClient(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITodoRepository, TodoRepository>()
                .AddSingleton<IUserRepository, UserRepository>();
        }

        private static IServiceCollection AddSqlClient(this IServiceCollection services, IConfiguration configuration)
        {
            //var fluentConnectionStringBuilder = new FluentConnectionStringBuilder();

            //var connectionString = fluentConnectionStringBuilder
            //    .AddServer("localhost")
            //    .AddPort(3306)
            //    .AddUserId("userOleg")
            //    .AddPassword("rootroot")
            //    .AddDatabase("todoitemsapp")
            //    .BuildConnectionString(true);

            //I method
            var connectionString = configuration.GetSection("ConnectionStrings")["SqlConnectionString"];

            //II method
            //var connectionString = configuration.GetSection("ConnectionStrings").GetSection("SqlConnectionString").Value;

            //III method
            //var connectionString = configuration.GetSection("ConnectionStrings:SqlConnectionString").Value;

            //IV method
            //var connectionString = configuration.GetConnectionString("SqlConnectionString");

            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }
    }
}
