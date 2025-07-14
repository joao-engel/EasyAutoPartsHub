namespace EasyAutoPartsHub.Models;
public class UsuarioModel
{
    public int? ID { get; set; }
    public string Nome { get; set; }
    public string Usuario { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Salt { get; set; }
    public DateTime DataCadastro { get; set; }
}
