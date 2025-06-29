using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services;

public interface IRelatorioServices
{
    Task<List<RelFaturamentoProdutoModel>> FaturamentoProduto(DateTime dataIni, DateTime dataFim);
    Task<List<RelFaturamentoClienteModel>> FaturamentoCliente(DateTime dataIni, DateTime dataFim);
    Task<List<RelOrcamentoStatusModel>> OrcamentoStatus(DateTime dataIni, DateTime dataFim);
}

public class RelatorioServices : IRelatorioServices
{
    private readonly IRelatorioRepository _relatorioRepository;

    public RelatorioServices(IRelatorioRepository relatorioRepository)
    {
        _relatorioRepository = relatorioRepository;
    }

    public async Task<List<RelFaturamentoProdutoModel>> FaturamentoProduto(DateTime dataIni, DateTime dataFim)
    {
        return await _relatorioRepository.FaturamentoProduto(dataIni, dataFim);
    }

    public async Task<List<RelFaturamentoClienteModel>> FaturamentoCliente(DateTime dataIni, DateTime dataFim)
    {
        return await _relatorioRepository.FaturamentoCliente(dataIni, dataFim);
    }

    public async Task<List<RelOrcamentoStatusModel>> OrcamentoStatus(DateTime dataIni, DateTime dataFim)
    {
        return await _relatorioRepository.OrcamentoStatus(dataIni, dataFim);
    }
}
