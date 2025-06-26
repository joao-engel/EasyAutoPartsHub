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
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoServices, PedidoServices>();
            services.AddScoped<IMetasRepository, MetasRepository>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
            services.AddScoped<IOrcamentoServices, OrcamentoServices>();
            services.AddScoped<IOrcamentoParaPedidoServices, OrcamentoParaPedidoServices>();
            services.AddScoped<IRelatorioRepository, RelatorioRepository>();
            services.AddScoped<IRelatorioServices, RelatorioServices>();

            return services;
        }
    }
}