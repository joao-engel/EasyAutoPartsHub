using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IOrcamentoServices
    {
        Task<List<StatusModel>> ListarStatus();
        Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model);
    }

    public class OrcamentoServices : IOrcamentoServices
    {
        private readonly IOrcamentoRepository _orcamentoRepository;

        public OrcamentoServices(IOrcamentoRepository orcamentoRepository)
        {
            _orcamentoRepository = orcamentoRepository;
        }

        public async Task<List<StatusModel>> ListarStatus()
        {
            try
            {
                var ret = await _orcamentoRepository.ListarStatus();
                return [.. ret.OrderBy(x => x.Ordem)];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model)
        {
            try
            {
                return await _orcamentoRepository.ListarOrcamentos(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
