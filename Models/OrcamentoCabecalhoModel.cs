namespace EasyAutoPartsHub.Models;
public class OrcamentoCabecalhoModel
{
    public int? ID { get; set; }
    public int ClienteID { get; set; }
    public string Cliente { get; set; }
    public DateTime DataOrcamento { get; set; }
    public int StatusID { get; set; }
    public string Status { get; set; }
    public decimal ValorTotal { get; set; }
    public int QuantidadeItens { get; set; }
    public string Observacao { get; set; }
}
