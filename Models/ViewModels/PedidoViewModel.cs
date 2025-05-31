namespace EasyAutoPartsHub.Models.ViewModels;
public class PedidoViewModel
{
    public PedidoViewModel()
    {
        Pedido = new();
        Itens = [];
    }

    public PedidoCabecalhoModel Pedido { get; set; }
    public List<PedidoItemModel> Itens { get; set; }
}
