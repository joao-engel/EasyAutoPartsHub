using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services;

public interface IRelatorioServices
{
    Task<List<RelFaturamentoProdutoModel>> FaturamentoPeriodo(DateTime dataIni, DateTime dataFim);
}

public class RelatorioServices : IRelatorioServices
{
	private readonly IRelatorioRepository _relatorioRepository;

    public RelatorioServices(IRelatorioRepository relatorioRepository)
    {
        _relatorioRepository = relatorioRepository;
    }

    public async Task<List<RelFaturamentoProdutoModel>> FaturamentoPeriodo(DateTime dataIni, DateTime dataFim)
    {
		try
		{
			return await _relatorioRepository.FaturamentoPeriodo(dataIni, dataFim);
        }
		catch (Exception)
		{
			throw;
		}
    }
}
