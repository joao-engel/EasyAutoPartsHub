namespace EasyAutoPartsHub.Models;
public class ProdutoModel
{
    public int? ID { get; set; }
    public string CodigoExterno { get; set; }
    public string Descricao { get; set; }
    public int GrupoID { get; set; }
    public string Grupo { get; set; }
    public int FornecedorID { get; set; }
    public string Fornecedor { get; set; }
    public decimal? Preco { get; set; }
    public bool Ativo { get; set; }
}
