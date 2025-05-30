using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IFornecedorServices
    {
        Task<List<FornecedorModel>> Listar(FornecedorModel model);
        Task<FornecedorModel> Obter(int id);
        Task Salvar(FornecedorModel model);
    }

    public class FornecedorServices : IFornecedorServices
    {
        private readonly IFornecedorRepository _rep;

        public FornecedorServices(IFornecedorRepository rep)
        {
            _rep = rep;
        }

        public async Task<List<FornecedorModel>> Listar(FornecedorModel model)
        {
            return await _rep.Listar(model);
        }

        public async Task<FornecedorModel> Obter(int id)
        {
            try
            {
                List<FornecedorModel> ret = await Listar(new FornecedorModel { ID = id });
                return ret.Single();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Salvar(FornecedorModel model)
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
