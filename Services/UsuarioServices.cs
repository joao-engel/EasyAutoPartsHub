using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IUsuarioServices
    {
        Task<List<UsuarioModel>> Listar();
        Task<UsuarioModel> ObterPorId(int id);
        Task<UsuarioModel> ObterParaLogin(string usuario);
        Task Salvar(UsuarioModel model);
    }

    public class UsuarioServices : IUsuarioServices
    {
        private readonly IUsuarioRepository _repUsuarios;

        public UsuarioServices(IUsuarioRepository repUsuarios)
        {
            _repUsuarios = repUsuarios;
        }

        public async Task<List<UsuarioModel>> Listar()
        {
            return await _repUsuarios.Listar();
        }

        public async Task<UsuarioModel> ObterPorId(int id)
        {
            var usuarios = await _repUsuarios.Listar();
            return usuarios.FirstOrDefault(u => u.ID == id);
        }

        public async Task<UsuarioModel> ObterParaLogin(string usuario)
        {
            try
            {
                var usuarios = await _repUsuarios.Listar();

                UsuarioModel usuarioModel = new();

                usuarioModel = await ObterPorUsuario(usuarios, usuario);
                usuarioModel ??= await ObterPorEmail(usuarios, usuario);

                if (usuario == null)
                {
                    throw new Exception("Usuário não encontrado");
                }

                return usuarioModel;
            }
            catch (Exception)
            {
                throw;
            }
            
        }        

        public async Task Salvar(UsuarioModel model)
        {
            if (model.ID.HasValue)
            {
                // Atualizar usuário existente
                await _repUsuarios.Atualizar(model);
            }
            else
            {
                SenhaHelper.GerarHashSenha(model.Senha, out var hash, out var salt);
                model.Senha = hash;
                model.Salt = salt;

                await _repUsuarios.Inserir(model);
            }
        }

        private async Task<UsuarioModel> ObterPorEmail(List<UsuarioModel> usuarios, string email)
        {
            return usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<UsuarioModel> ObterPorUsuario(List<UsuarioModel> usuarios, string usuario)
        {
            return usuarios.FirstOrDefault(u => u.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
        }
    }
}
