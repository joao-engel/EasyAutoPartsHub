using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IProdutoServices
    {
        Task<List<ProdutoModel>> Listar(ProdutoRQModel model);
        Task<ProdutoModel> Obter(int id);
        Task Salvar(ProdutoModel model);
    }

    public class ProdutoServices : IProdutoServices
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoServices(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<List<ProdutoModel>> Listar(ProdutoRQModel model)
        {
            return await _produtoRepository.Listar(model);
        }

        public async Task<ProdutoModel> Obter(int id)
        {
            try
            {
                List<ProdutoModel> ret = await Listar(new ProdutoRQModel { ID = id });
                return ret.Single();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Salvar(ProdutoModel model)
        {
            try
            {
                if (model.ID.HasValue)
                {
                    await _produtoRepository.Atualizar(model);
                }
                else
                {
                    await _produtoRepository.Inserir(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
