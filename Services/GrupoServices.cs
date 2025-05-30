using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IGrupoServices
    {
        Task<List<GrupoProdutoModel>> Listar(GrupoProdutoModel model);
        Task<GrupoProdutoModel> Obter(int id);
        Task Salvar(GrupoProdutoModel model);
    }

    public class GrupoServices : IGrupoServices
    {
        private readonly IGrupoRepository _rep;

        public GrupoServices(IGrupoRepository rep)
        {
            _rep = rep;
        }

        public async Task<List<GrupoProdutoModel>> Listar(GrupoProdutoModel model)
        {
            try
            {
                return await _rep.Listar(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GrupoProdutoModel> Obter(int id)
        {
            try
            {
                List<GrupoProdutoModel> ret = await Listar(new GrupoProdutoModel { ID = id });
                return ret.Single();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Salvar(GrupoProdutoModel model)
        {
            try
            {
                if (model.ID.HasValue)
                {
                    await _rep.Atualizar(model);
                }
                else
                {
                    await _rep.Inserir(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
