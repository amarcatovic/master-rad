using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assemmbly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(config => 
                config.RegisterServicesFromAssembly(assemmbly));

            services.AddScoped<IUpdatePostCacheService, UpdatePostCacheService>();

            services.AddValidatorsFromAssembly(assemmbly);

            return services;
        }
    }
}
