namespace EasyAutoPartsHub.Models;

public class OrcamentoItemModel
{
    public int? ID { get; set; }
    public int OrcamentoID { get; set; }
    public int? ProdutoID { get; set; }
    public string Produto { get; set; }
    public string Grupo { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal SubTotal { get; set; }
}
