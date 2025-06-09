namespace EasyAutoPartsHub.Models;
public class DashboardHeaderModel
{
    public decimal FaturamentoMensal { get; set; }
    public decimal MetaMensal { get; set; }
    public decimal AtingMensal { get; set; }
    public decimal FaturamentoAnual { get; set; }
    public decimal MetaAnual { get; set; }
    public decimal AtingAnual { get; set; }
    public int PedidosTotal { get; set; }
    public int PedidosConcluidos { get; set; }
    public int PedidosCancelados { get; set; }
    public decimal PedidoValorMedio { get; set; }
    public decimal PedidoQuantidadeMedia { get; set; }
}
