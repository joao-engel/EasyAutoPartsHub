namespace EasyAutoPartsHub.Models;
public class DashboardUltimosPedidos
{
    public DashboardUltimosPedidos()
    {
        Pedidos = [];
    }

    public string Status { get; set; }
    public List<PedidoCabecalhoModel> Pedidos { get; set; }
}
