using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using EasyAutoPartsHub.Repository.ExceptionCustom;

namespace EasyAutoPartsHub.Repository
{
    public interface IClienteRepository
    {
        Task<List<ClienteModel>> Listar(ClienteModel model);
        Task Inserir(ClienteModel model);
        Task Atualizar(ClienteModel model);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly IDapperService _dapper;

        public ClienteRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<ClienteModel>> Listar(ClienteModel model)
        {
            try
            {
                string sql = @"
SELECT
	ID,
	Nome,
	Tipo,
	Documento,
	Telefone
FROM EasyAutoPartsHubDb.dbo.Cliente
WHERE (@ID IS NULL OR ID = @ID)
AND (@Nome IS NULL OR Nome LIKE @Nome)
AND (@Tipo IS NULL OR Tipo = @Tipo)
AND (@Documento IS NULL OR Documento LIKE @Documento)
AND (@Telefone IS NULL OR Telefone LIKE @Telefone)
";
                return await _dapper.QueryAsync<ClienteModel>(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(ClienteModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.Cliente
(Nome,
Tipo,
Documento,
Telefone)
VALUES
(@Nome,
@Tipo,
@Documento,
@Telefone)
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryCliente());
            }
        }

        public async Task Atualizar(ClienteModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.Cliente SET
Nome = @Nome,
Tipo = @Tipo,
Documento = @Documento,
Telefone = @Telefone
WHERE ID = @ID
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryCliente());
            }
        }
    }
}
