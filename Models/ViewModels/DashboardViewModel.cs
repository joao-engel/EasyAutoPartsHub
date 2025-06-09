namespace EasyAutoPartsHub.Models.ViewModels;
public class DashboardViewModel
{
    public DashboardViewModel()
    {
        Header = new();
        Graficos = new();
        UltimosPedidos = [];
        Ranking = new();
    }

    public DashboardHeaderModel Header { get; set; }
    public DashboardGraficosPedidosModel Graficos { get; set; }
    public List<DashboardUltimosPedidos> UltimosPedidos { get; set; }
    public DashboardRankingModel Ranking { get; set; }
}
