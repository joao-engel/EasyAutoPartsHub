using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IClienteServices
    {
        Task<List<ClienteModel>> Listar(ClienteModel model);
        Task<ClienteModel> Obter(int id);
        Task Salvar(ClienteModel model);
    }

    public class ClienteServices : IClienteServices
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteServices(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        
        public async Task<List<ClienteModel>> Listar(ClienteModel model)
        {
            return await _clienteRepository.Listar(model);
        }

        public async Task<ClienteModel> Obter(int id)
        {
            try
            {
                List<ClienteModel> ret = await Listar(new ClienteModel { ID = id });
                return ret.Single();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Salvar(ClienteModel model)
        {
            try
            {
                if (model.ID.HasValue)
                {
                    await _clienteRepository.Atualizar(model);
                }
                else
                {
                    await _clienteRepository.Inserir(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
