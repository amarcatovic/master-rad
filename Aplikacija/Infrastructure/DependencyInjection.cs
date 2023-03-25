using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StackOverflow2010Context>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(StackOverflow2010Context).Assembly.FullName).CommandTimeout(120)));

            services.AddDbContext<PostCacheDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PostCacheConnection")));

            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { configuration.GetConnectionString("Redis") },
                    Password = "AZ4N38hJbEu(MeG]_{E",
                    DefaultDatabase = 7
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddSingleton<IMongoDBClient, MongoDBClient>();

            return services;
        }
    }
}
