using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Models.ViewModels;
using EasyAutoPartsHub.Repository;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Transactions;

namespace EasyAutoPartsHub.Services;

public interface IPedidoServices
{
    Task<List<PedidoStatusModel>> ListarStatus();
    Task<List<PedidoCabecalhoModel>> ListarPedidos(PedidoCabecalhoRQModel model);
    Task<PedidoCabecalhoModel> ObterPedido(int id);
    Task<PedidoViewModel> VisualizarPedido(int pedidoID);
    Task Salvar(PedidoCadastroModel model);
    Task AlterarSituacao(PedidoAlterarStatusModel model);
    Task CancelarPedido(PedidoAlterarStatusModel model);
}

public class PedidoServices : IPedidoServices
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoServices _produtoServices;

    public PedidoServices(IPedidoRepository pedidoRepository, IProdutoServices produtoServices)
    {
        _pedidoRepository = pedidoRepository;
        _produtoServices = produtoServices;
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

    public async Task Salvar(PedidoCadastroModel model)
    {
        try
        {
            List<PedidoItemCadastroModel> lstItens = [];
            PedidoCabecalhoModel pedCabecalho = new()
            {
                ClienteID = model.ClienteID.Value,
                Observacao = model.Observacao,
                StatusID = 1, // Status "Pendente"
                DataEmissao = DateTime.Now
            };

            List<ProdutoModel> lstProdutos = await _produtoServices.Listar(new ProdutoRQModel { Ativo = true });

            foreach (var item in model.Produtos)
            {
                if (!item.ID.HasValue)
                {
                    throw new Exception($"Erro ao encontrar o produto! ID: {item.ID}");
                }

                ProdutoModel produto = lstProdutos.SingleOrDefault(p => p.ID == item.ID.Value);
                if (!produto.Preco.HasValue)
                {
                    throw new Exception($"Erro ao lançar o produto {produto.Descricao}! Cadastre um preço e tente novamente!");
                }

                PedidoItemCadastroModel tempItem = new()
                {
                    ProdutoID = item.ID.Value,
                    Quantidade = item.Quantidade,
                    ValorUnitario = produto.Preco.Value
                };

                lstItens.Add(tempItem);
            }

            await InserirPedido(pedCabecalho, lstItens);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task AlterarSituacao(PedidoAlterarStatusModel model)
    {
        try
        {
            ValidarData(model.Data);

            PedidoCabecalhoModel pedido = await ObterPedido(model.ID)
                ?? throw new Exception("Pedido não encontrado!");

            switch (pedido.StatusID)
            {
                case 1:
                    if (model.Data < pedido.DataEmissao)
                        throw new Exception("A data de faturamento não pode ser anterior à data de emissão.");

                    await _pedidoRepository.AlterarStatusParaFaturado(model);
                    return;

                case 2:
                    if (model.Data < pedido.DataFaturamento)
                        throw new Exception("A data de faturamento não pode ser anterior à data de faturamento atual.");

                    await _pedidoRepository.AlterarStatusParaEntregue(model);
                    return;

                default:
                    throw new Exception($"O status {pedido.Status} não permite alteração!");
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CancelarPedido(PedidoAlterarStatusModel model)
    {
        try
        {
            ValidarData(model.Data);

            PedidoCabecalhoModel pedido = await ObterPedido(model.ID)
                ?? throw new Exception("Pedido não encontrado!");

            if (pedido.StatusID != 1 && pedido.StatusID != 2)
                throw new Exception($"O status {pedido.Status} não permite cancelamento!");

            if (pedido.DataFaturamento.HasValue)
            {
                if (model.Data < pedido.DataFaturamento.Value)
                    throw new Exception("A data de cancelamento não pode ser anterior que de faturamento");
            }
            else if (model.Data < pedido.DataEmissao)
            {
                throw new Exception("A data de cancelamento não pode ser anterior que de emissão");
            }

            await _pedidoRepository.CancelarPedido(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task InserirPedido(PedidoCabecalhoModel pedCabecalho, List<PedidoItemCadastroModel> lstItens)
    {
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            int pedidoID = await _pedidoRepository.InserirPedidoCabecalho(pedCabecalho);

            foreach (var item in lstItens)
            {
                item.PedidoID = pedidoID;
                await _pedidoRepository.InserirPedidoItem(item);
            }

            scope.Complete();
        }
        catch (Exception ex)
        {
            scope.Dispose();
            throw new Exception($"Erro ao inserir pedido<br><hr>{ex.Message}");
        }
    }

    private static void ValidarData(DateTime data)
    {
        if (data > DateTime.Now)
            throw new Exception("Não pode ser usado data futura!");
    }
}
