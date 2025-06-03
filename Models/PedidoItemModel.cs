namespace EasyAutoPartsHub.Models;
public class PedidoItemModel
{
    public int? ID { get; set; }
    public string Produto { get; set; }
    public string Grupo { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal SubTotal { get; set; }
}
