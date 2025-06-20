using EasyAutoPartsHub.Lib;
using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;
using Org.BouncyCastle.Utilities;
using System.Text;
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
        Task<string> GerarHtmlOrcamento(int id);
    }

    public class OrcamentoServices : IOrcamentoServices
    {
        private readonly IOrcamentoRepository _orcamentoRepository;
        private readonly IProdutoServices _produtoServices;
        private readonly IClienteServices _clienteServices;

        public OrcamentoServices(IOrcamentoRepository orcamentoRepository, IProdutoServices produtoServices, IClienteServices clienteServices)
        {
            _orcamentoRepository = orcamentoRepository;
            _produtoServices = produtoServices;
            _clienteServices = clienteServices;
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
                    DataOrcamento = orcamento.DataOrcamento,
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

        public async Task<string> GerarHtmlOrcamento(int id)
        {
            try
            {
                OrcamentoCadastroModel orcamento = await ObterOrcamentoCadastro(id);
                ClienteModel cliente = await _clienteServices.Obter(orcamento.ClienteID.Value);

                string tipoDoc = cliente.Tipo == "PF" ? "CPF" : "CNPJ";

                string body = Body.Template("Orcamento.html");
                body = body.Replace("#ID#", orcamento.ID.Value.ToString())
                            .Replace("#DataOrcamento#", orcamento.DataOrcamento.ToShortDateString())
                            .Replace("#Cliente#", orcamento.Cliente)
                            .Replace("#TipoDoc#", tipoDoc)
                            .Replace("#Doc#", cliente.Documento)
                            .Replace("#Contato#", cliente.Telefone)
                            .Replace("#TotalOrcamento#", orcamento.Produtos.Sum(x => x.SubTotal).ToString("N2"))
                            .Replace("#Observacao#", orcamento.Observacao);

                StringBuilder sbItens = new();
                int linha = 1;

                foreach (var item in orcamento.Produtos)
                {
                    decimal subtotal = item.Quantidade * item.ValorUnitario;

                    sbItens.AppendLine($"<tr>" +
                        $"<td>{linha}</td>" +
                        $"<td>{item.Produto}</td>" +
                        $"<td>{item.Quantidade}</td>" +
                        $"<td>{item.ValorUnitario.ToString("N2")}</td>" +
                        $"<td>{subtotal.ToString("N2")}</td>" +
                        $"</tr>");

                    linha++;
                }

                body = body.Replace("#Itens#", sbItens.ToString());

                var logo = Body.Logo("logo-dark.png");
                var base64 = Convert.ToBase64String(logo);
                body = body.Replace("#base64img#", base64);

                return body;
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
