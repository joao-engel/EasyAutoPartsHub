namespace EasyAutoPartsHub.Models;
public class OrcamentoCadastroModel
{
    public int? ID { get; set; }
    public int? ClienteID { get; set; }
    public string Cliente { get; set; }
    public string Observacao { get; set; }
    public List<OrcamentoItemModel> Produtos { get; set; }
}
