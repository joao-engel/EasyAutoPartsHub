using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;
using System.Transactions;

namespace EasyAutoPartsHub.Services
{
    public interface IOrcamentoParaPedidoServices
    {
        Task GerarPedido(int id);
    }

    public class OrcamentoParaPedidoServices : IOrcamentoParaPedidoServices
    {
        private readonly IOrcamentoServices _orcamentoServices;
        private readonly IOrcamentoRepository _orcamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public OrcamentoParaPedidoServices(IOrcamentoServices orcamentoServices, IPedidoRepository pedidoRepository, IOrcamentoRepository orcamentoRepository)
        {
            _orcamentoServices = orcamentoServices;
            _pedidoRepository = pedidoRepository;
            _orcamentoRepository = orcamentoRepository;
        }

        public async Task GerarPedido(int id)
        {
            try
            {
                OrcamentoCadastroModel orcamento = await _orcamentoServices.ObterOrcamentoCadastro(id);

                PedidoCabecalhoModel pedidoCabecalho = new()
                {
                    ClienteID = orcamento.ClienteID.Value,
                    DataEmissao = DateTime.Now,
                    StatusID = 1,
                    Observacao = $"Pedido gerado através do orçamento ID: {orcamento.ID.Value}"
                };

                List<PedidoItemCadastroModel> lstItens = [.. orcamento.Produtos.Select(item => new PedidoItemCadastroModel
                {
                    ProdutoID = item.ProdutoID.Value,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                })];

                await InserirPedido(pedidoCabecalho, lstItens, id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InserirPedido(PedidoCabecalhoModel pedidoCabecalho, List<PedidoItemCadastroModel> lstItens, int orcamentoID)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _orcamentoRepository.ConcluirOrcamento(orcamentoID);

                int pedidoID = await _pedidoRepository.InserirPedidoCabecalho(pedidoCabecalho);
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
    }
}
