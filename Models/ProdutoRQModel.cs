namespace EasyAutoPartsHub.Models;
public class ProdutoRQModel
{
    public int? ID { get; set; }
    public string CodigoExterno { get; set; }
    public string Descricao { get; set; }
    public int? GrupoID { get; set; }
    public int? FornecedorID { get; set; }
    public bool? Ativo { get; set; }
}
