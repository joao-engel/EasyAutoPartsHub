using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Models.ViewModels;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services;

public interface IPedidoServices
{
    Task<List<PedidoStatusModel>> ListarStatus();
    Task<List<PedidoCabecalhoModel>> ListarPedidos(PedidoCabecalhoRQModel model);
    Task<PedidoCabecalhoModel> ObterPedido(int id);
    Task<PedidoViewModel> VisualizarPedido(int pedidoID);
}

public class PedidoServices : IPedidoServices
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoServices(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<List<PedidoStatusModel>> ListarStatus()
    {
        return await _pedidoRepository.ListarStatus();
    }

    public async Task<List<PedidoCabecalhoModel>> ListarPedidos(PedidoCabecalhoRQModel model)
    {
        return await _pedidoRepository.ListarPedidos(model);
    }

    public async Task<PedidoCabecalhoModel> ObterPedido(int id)
    {
        try
        {
            List<PedidoCabecalhoModel> ret = await ListarPedidos(new PedidoCabecalhoRQModel { ID = id });
            return ret.Single();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PedidoViewModel> VisualizarPedido(int pedidoID)
    {
        try
        {
            PedidoViewModel vm = new()
            {
                Pedido = await ObterPedido(pedidoID),
                Itens = await _pedidoRepository.VisualizarPedido(pedidoID)
            };

            return vm;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao visualizar o pedido: {ex.Message}");
        }
    }
}
