using EasyAutoPartsHub.Repository;
using EasyAutoPartsHub.Repository.Dapper;
using EasyAutoPartsHub.Services;

namespace EasyAutoPartsHub
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IDapperService, DapperService>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<IGrupoServices, GrupoServices>();


            return services;
        }
    }
}