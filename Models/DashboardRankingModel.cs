namespace EasyAutoPartsHub.Models;

public class DashboardRankingModel
{
    public DashboardRankingModel()
    {
        Clientes = [];
    }
    public List<RankingClientesModel> Clientes { get; set; }
}

public class RankingClientesModel
{
    public int Posicao { get; set; }
    public string Cliente { get; set; }
    public decimal Valor { get; set; }
}
