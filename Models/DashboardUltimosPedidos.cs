namespace EasyAutoPartsHub.Models;
public class DashboardUltimosPedidos
{
    public DashboardUltimosPedidos()
    {
        Pedidos = [];
    }

    public string Status { get; set; }
    public List<UltimosPedidosModel> Pedidos { get; set; }
}

public class UltimosPedidosModel
{
    public int ID { get; set; }
    public string Cliente { get; set; }
    public string Status { get; set; }
    public DateTime DataEmissao { get; set; }
    public decimal Valor { get; set; }
}