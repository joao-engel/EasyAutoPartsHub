using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Models.ViewModels;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IDashboardServices
    {
        Task<DashboardViewModel> Dashboard(int ano, int mes);
    }

    public class DashboardServices(IPedidoRepository pedidoRepository, IMetasRepository metasRepository)  : IDashboardServices
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
        private readonly IMetasRepository _metasRepository = metasRepository;

        public async Task<DashboardViewModel> Dashboard(int ano, int mes)
        {
            try
            {
                ValidarAnoMes(ano, mes);
                DashboardViewModel ret = new();

                List<PedidoCabecalhoModel> lstPedidos = await _pedidoRepository.ListarPedidos(new PedidoCabecalhoRQModel { Ano = ano });
                List<MetaModel> lstMetas = await _metasRepository.Listar(new MetaModel { Ano = ano });

                Header(ret.Header, lstPedidos, lstMetas, mes);
                Faturamentos(ret.Graficos, lstPedidos, lstMetas, mes, ano);
                Status(ret.Graficos, lstPedidos, mes);
                UltimosPedidos(ret.UltimosPedidos, lstPedidos);
                Ranking(ret.Ranking, lstPedidos);

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Header(DashboardHeaderModel header, List<PedidoCabecalhoModel> lstPedidos, List<MetaModel> lstMetas, int mes)
        {
            try
            {
                int[] statusConcluido = [2, 3]; // Status de pedidos concluídos (Faturado, Entregue)
                List<PedidoCabecalhoModel> pedidosFaturadosMensal = [.. lstPedidos.Where(x => statusConcluido.Contains(x.StatusID) && x.DataFaturamento.Value.Month == mes)];
                List<PedidoCabecalhoModel> pedidosFaturadosAnual = [.. lstPedidos.Where(x => statusConcluido.Contains(x.StatusID))];

                //Mensal
                header.FaturamentoMensal = pedidosFaturadosMensal.Sum(x => x.ValorTotal);
                header.MetaMensal = lstMetas.SingleOrDefault(x => x.Mes == mes)?.Valor ?? 0;
                header.AtingMensal = header.MetaMensal > 0 ? (header.FaturamentoMensal / header.MetaMensal) * 100 : 0;

                //Anual
                header.FaturamentoAnual = pedidosFaturadosAnual.Sum(x => x.ValorTotal);
                header.MetaAnual = lstMetas.Sum(x => x.Valor);
                header.AtingAnual = header.MetaAnual > 0 ? (header.FaturamentoAnual / header.MetaAnual) * 100 : 0;

                //Contagem de Pedidos
                header.PedidosTotal = lstPedidos.Count;
                header.PedidosConcluidos = pedidosFaturadosAnual.Count;
                header.PedidosCancelados = lstPedidos.Count(x => x.StatusID == 4);
                header.PedidosEmAberto = lstPedidos.Count(x => x.StatusID == 1);

                //Médias
                header.PedidoValorMedio = header.PedidosConcluidos > 0 ? header.FaturamentoAnual / header.PedidosConcluidos : 0;
                header.PedidoQuantidadeMedia = header.PedidosConcluidos > 0 ? pedidosFaturadosAnual.Sum(x => x.QuantidadeItens) / header.PedidosConcluidos : 0;
                header.ProdutosVendidos = pedidosFaturadosAnual.Sum(x => x.QuantidadeItens);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao calcular o header<br><hr><small>{ex.Message}</small>");
            }
        }
        
        private static void Faturamentos(DashboardGraficosPedidosModel ret, List<PedidoCabecalhoModel> lstPedidos, List<MetaModel> lstMetas, int mes, int ano)
        {
            try
            {
                int[] statusConcluido = [2, 3];
                List<PedidoCabecalhoModel> pedidosFaturadosAnual = [.. lstPedidos.Where(x => statusConcluido.Contains(x.StatusID))];
                List<PedidoCabecalhoModel> pedidosFaturadosMensal = [.. pedidosFaturadosAnual.Where(x => x.DataFaturamento.Value.Month == mes)];

                var pedidosAgrupadosAno = pedidosFaturadosAnual
                                            .Where(p => p.DataFaturamento.HasValue)
                                            .GroupBy(p => new { p.DataFaturamento.Value.Year, p.DataFaturamento.Value.Month })
                                            .Select(g => new DashboardFaturamentoPedidosModel
                                            {
                                                Data = DataHelper.GetMesAbreviado(g.Key.Month),
                                                Valor = g.Sum(p => p.ValorTotal),
                                                Meta = lstMetas.FirstOrDefault(x => x.Mes == g.Key.Month).Valor,
                                                Ordem = 100 + g.Key.Month
                                            })
                                            .OrderBy(x => x.Ordem)
                                            .ToList();

                // Calcula o valor da meta diária
                decimal metaDiaria = (lstMetas.FirstOrDefault(x => x.Mes == mes).Valor) / DateTime.DaysInMonth(ano, mes);

                // Obtém o maior dia faturado no mês
                int ultimoDiaComFaturamento = pedidosFaturadosMensal
                    .Select(p => p.DataFaturamento.Value.Day)
                    .DefaultIfEmpty(1)
                    .Max();

                // Agrupa os pedidos mensais pelos dias faturados
                var pedidosAgrupadosPorDia = pedidosFaturadosMensal
                    .GroupBy(p => p.DataFaturamento.Value.Day)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.ValorTotal));

                // Gera a lista completa do dia 1 até o último dia com faturamento
                var pedidosAgrupadosMes = Enumerable.Range(1, ultimoDiaComFaturamento)
                    .Select(dia => new DashboardFaturamentoPedidosModel
                    {
                        Data = dia.ToString(),
                        Valor = pedidosAgrupadosPorDia.TryGetValue(dia, out decimal value) ? value : 0, // caso não encontre valor naquele dia, considera 0
                        Meta = metaDiaria,
                        Ordem = 100 + dia
                    })
                    .OrderBy(x => x.Ordem)
                    .ToList();

                ret.FaturamentosMensal = pedidosAgrupadosMes;
                ret.FaturamentosAnual = pedidosAgrupadosAno;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao calcular o faturamento de forma detalhada<br><hr><small>{ex.Message}</small>");
            }
        }

        private static void Status(DashboardGraficosPedidosModel ret, List<PedidoCabecalhoModel> lstPedidos, int mes)
        {
            try
            {
                List<PedidoCabecalhoModel> lstPedidosMensal = [.. lstPedidos.Where(x => x.DataEmissao.Month == mes)];

                List<DashboardPedidoStatusModel> statusMensal = [.. lstPedidosMensal.GroupBy(x => x.Status)
                                                                            .Select(x => new DashboardPedidoStatusModel
                                                                            {
                                                                                Status = x.Key,
                                                                                Quantidade = x.Count()
                                                                            })];

                List<DashboardPedidoStatusModel> statusAnual = [.. lstPedidos.GroupBy(x => x.Status)
                                                                            .Select(x => new DashboardPedidoStatusModel
                                                                            {
                                                                                Status = x.Key,
                                                                                Quantidade = x.Count()
                                                                            })];

                ret.StatusMensal = statusMensal;
                ret.StatusAnual = statusAnual;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao agrupar os status de forma detalhada<br><hr><small>{ex.Message}</small>");
            }
        }

        private static void UltimosPedidos(List<DashboardUltimosPedidos> ret, List<PedidoCabecalhoModel> lstPedidos)
        {
            try
            {
                var resultado = lstPedidos
                                    .GroupBy(p => p.Status)
                                    .Select(g => new DashboardUltimosPedidos
                                    {
                                        Status = g.Key,
                                        Pedidos = g.OrderByDescending(p => p.DataEmissao)
                                                   .Take(10)
                                                   .Select(p => new UltimosPedidosModel
                                                   {
                                                       ID = p.ID.Value,
                                                       Cliente = p.Cliente,
                                                       Status = p.Status,
                                                       DataEmissao = p.DataEmissao,
                                                       Valor = p.ValorTotal
                                                   }).ToList()
                                    })
                                    .ToList();

                var todos = new DashboardUltimosPedidos
                                {
                                    Status = "Todos",
                                    Pedidos = lstPedidos
                                        .OrderByDescending(p => p.DataFaturamento)
                                        .Take(10)
                                        .Select(p => new UltimosPedidosModel
                                        {
                                            ID = p.ID ?? 0,
                                            Cliente = p.Cliente,
                                            Status = p.Status,
                                            DataEmissao = p.DataEmissao,
                                            Valor = p.ValorTotal
                                        }).ToList()
                                };

                ret.Add(todos);
                ret.AddRange(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar os últimos pedidos<br><hr><small>{ex.Message}</small>");
            }
        }

        private static void Ranking(DashboardRankingModel ret, List<PedidoCabecalhoModel> lstPedidos)
        {
            try
            {
                var clientes = lstPedidos
                                .GroupBy(p => p.Cliente)
                                .Select(g => new
                                {
                                    Cliente = g.Key,
                                    Valor = g.Sum(p => p.ValorTotal)
                                })
                                .OrderByDescending(x => x.Valor)
                                .Take(5)
                                .Select((x, index) => new RankingClientesModel
                                {
                                    Posicao = index + 1,
                                    Cliente = x.Cliente,
                                    Valor = x.Valor
                                })
                                .ToList();

                ret.Clientes = clientes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao definir o ranking<br><hr><small>{ex.Message}</small>");
            }
        }
        
        private static void ValidarAnoMes(int ano, int mes)
        {
            if (ano < 2020 || ano > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(ano), "O ano deve estar entre 2020 e o ano atual.");
            if (mes < 1 || mes > 12)
                throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");
        }
    }
}
