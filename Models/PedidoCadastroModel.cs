using EasyAutoPartsHub.Models;

public class PedidoCadastroModel
{
    public int? ClienteID { get; set; }
    public string Observacao { get; set; }

    public List<PedidoItemModel> Produtos { get; set; }
}