using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository;

namespace EasyAutoPartsHub.Services
{
    public interface IUsuarioServices
    {
        Task<List<UsuarioModel>> Listar();
        Task<UsuarioModel> ObterPorId(int id);
        Task<UsuarioModel> ObterPorEmail(string email);
        Task<UsuarioModel> ObterPorUsuario(string usuario);
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

        public async Task<UsuarioModel> ObterPorEmail(string email)
        {
            var usuarios = await _repUsuarios.Listar();
            return usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UsuarioModel> ObterPorUsuario(string usuario)
        {
            var usuarios = await _repUsuarios.Listar();
            return usuarios.FirstOrDefault(u => u.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
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
    }
}
