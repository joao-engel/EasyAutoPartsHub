namespace EasyAutoPartsHub.Models;
public class DashboardGraficosPedidosModel
{
    public DashboardGraficosPedidosModel()
    {
        FaturamentosMensal = []; 
        FaturamentosAnual = [];
        StatusMensal = [];
        StatusAnual = [];
    }

    public List<DashboardFaturamentoPedidosModel> FaturamentosMensal { get; set; }
    public List<DashboardFaturamentoPedidosModel> FaturamentosAnual { get; set; }
    public List<DashboardPedidoStatusModel> StatusMensal { get; set; }
    public List<DashboardPedidoStatusModel> StatusAnual { get; set; }
}
