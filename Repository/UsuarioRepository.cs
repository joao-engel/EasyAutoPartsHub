using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;

namespace EasyAutoPartsHub.Repository
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> Listar();
        Task Inserir(UsuarioModel model);
        Task Atualizar(UsuarioModel model);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDapperService _dapper;

        public UsuarioRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<UsuarioModel>> Listar()
        {
            try
            {
                string sql = @"
SELECT 
	ID,
	Nome,
    Usuario,
	Email,
	Senha,
	Salt,
	DataCadastro
FROM EasyAutoPartsHubDb.dbo.Usuario
";
                return await _dapper.QueryAsync<UsuarioModel>(sql: sql, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(UsuarioModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.Usuario
(Nome, Email, Usuario, Senha, Salt, DataCadastro)
VALUES
(@Nome, @Email, @Usuario, @Senha, @Salt, @DataCadastro)
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Atualizar(UsuarioModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.Usuario SET
Nome = @Nome,
Email = @Email,
Usuario = @Usuario,
DataCadastro = @DataCadastro
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
