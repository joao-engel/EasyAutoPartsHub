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
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IFornecedorServices, FornecedorServices>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoServices, ProdutoServices>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IClienteServices, ClienteServices>();

            return services;
        }
    }
}