using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace EasyAutoPartsHub.Services
{
    public interface IOrcamentoServices
    {
        Task<List<StatusModel>> ListarStatus();
        Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model);
        Task<List<OrcamentoItemModel>> ListarItensPorOrcamento(int id);
        Task<OrcamentoCadastroModel> ObterOrcamentoCadastro(int id);
        Task Salvar(OrcamentoCadastroModel model);
    }

    public class OrcamentoServices : IOrcamentoServices
    {
        private readonly IOrcamentoRepository _orcamentoRepository;
        private readonly IProdutoServices _produtoServices;

        public OrcamentoServices(IOrcamentoRepository orcamentoRepository, IProdutoServices produtoServices)
        {
            _orcamentoRepository = orcamentoRepository;
            _produtoServices = produtoServices;
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

        public async Task<List<OrcamentoItemModel>> ListarItensPorOrcamento(int id)
        {
            return await _orcamentoRepository.ListarItensPorOrcamento(id);
        }

        public async Task<OrcamentoCadastroModel> ObterOrcamentoCadastro(int id)
        {
            try
            {
                OrcamentoCabecalhoModel orcamento = await ObterOrcamentoCabecalho(id);

                OrcamentoCadastroModel ret = new()
                {
                    ID = orcamento.ID,
                    ClienteID = orcamento.ClienteID,
                    Cliente = orcamento.Cliente,
                    Observacao = orcamento.Observacao,
                    Produtos = await ListarItensPorOrcamento(id)
                };

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Salvar(OrcamentoCadastroModel model)
        {
            try
            {
                List<OrcamentoItemModel> lstItens = [];
                OrcamentoCabecalhoModel orcCabecalho = new()
                {
                    ID = model.ID,
                    ClienteID = model.ClienteID.Value,
                    Observacao = model.Observacao,
                    StatusID = 1, // Status "Rascunho"
                    DataOrcamento = DateTime.Now
                };

                List<ProdutoModel> lstProdutos = await _produtoServices.Listar(new ProdutoRQModel { Ativo = true });

                foreach (var item in model.Produtos)
                {
                    if (!item.ProdutoID.HasValue)
                    {
                        throw new Exception($"Erro ao encontrar o produto! ID: {item.ID}");
                    }

                    ProdutoModel produto = lstProdutos.SingleOrDefault(p => p.ID == item.ProdutoID.Value);
                    if (!produto.Preco.HasValue)
                    {
                        throw new Exception($"Erro ao lançar o produto {produto.Descricao}! Cadastre um preço e tente novamente!");
                    }

                    OrcamentoItemModel tempItem = new()
                    {
                        ProdutoID = item.ProdutoID.Value,
                        Quantidade = item.Quantidade,
                        ValorUnitario = produto.Preco.Value
                    };

                    lstItens.Add(tempItem);
                }

                if (orcCabecalho.ID.HasValue)
                {
                    await AtualizarOrcamento(orcCabecalho, lstItens);
                }
                else
                {
                    await InserirOrcamento(orcCabecalho, lstItens);
                }                    
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<OrcamentoCabecalhoModel> ObterOrcamentoCabecalho(int id)
        {
            try
            {
                List<OrcamentoCabecalhoModel> lst = await ListarOrcamentos(new OrcamentoCabecalhoRQModel { ID = id });
                return lst.Single();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InserirOrcamento(OrcamentoCabecalhoModel orcCabecalho, List<OrcamentoItemModel> lstItens)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                int orcamentoID = await _orcamentoRepository.InserirOrcamentoCabecalho(orcCabecalho);

                foreach (var item in lstItens)
                {
                    item.OrcamentoID = orcamentoID;
                    await _orcamentoRepository.InserirOrcamentoItem(item);
                }

                scope.Complete();
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw new Exception($"Erro ao inserir pedido<br><hr>{ex.Message}");
            }
        }

        private async Task AtualizarOrcamento(OrcamentoCabecalhoModel orcCabecalho, List<OrcamentoItemModel> lstItens)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _orcamentoRepository.AtualizarOrcamentoCabecalho(orcCabecalho);

                await _orcamentoRepository.DeletarItensPorOrcamento(orcCabecalho.ID.Value);

                foreach (var item in lstItens)
                {
                    item.OrcamentoID = orcCabecalho.ID.Value;
                    await _orcamentoRepository.InserirOrcamentoItem(item);
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
