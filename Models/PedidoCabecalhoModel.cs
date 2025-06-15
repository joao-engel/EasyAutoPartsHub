namespace EasyAutoPartsHub.Models;
public class PedidoCabecalhoModel
{
    public int? ID { get; set; }
    public int ClienteID { get; set; }
    public string Cliente { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime? DataFaturamento { get; set; }
    public DateTime? DataCancelado { get; set; }
    public DateTime? DataEntregue { get; set; }
    public int StatusID { get; set; }
    public string Status { get; set; }
    public decimal ValorTotal { get; set; }
    public int QuantidadeItens { get; set; }
    public string Observacao { get; set; }
}
