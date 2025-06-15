using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using System.Data;

namespace EasyAutoPartsHub.Repository
{
    public interface IMetasRepository
    {
        Task<List<MetaModel>> Listar(MetaModel model);
    }

    public class MetasRepository : IMetasRepository
    {
        private readonly IDapperService _dapper;

        public MetasRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<MetaModel>> Listar(MetaModel model)
        {
            try
            {
                string sql = @"
SELECT
	ID
	Mes,
	Ano,
	Valor
FROM EasyAutoPartsHubDb.dbo.Meta
WHERE (@ID IS NULL OR ID = @ID)
AND (@Mes IS NULL OR Mes = @Mes)
AND (@Ano IS NULL OR Ano = @Ano)
";
                return await _dapper.QueryAsync<MetaModel>(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
