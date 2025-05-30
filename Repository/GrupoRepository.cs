using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using EasyAutoPartsHub.Repository.ExceptionCustom;
using System.Data;

namespace EasyAutoPartsHub.Repository
{
    public interface IGrupoRepository
    {
        Task<List<GrupoProdutoModel>> Listar(GrupoProdutoModel model);
        Task Inserir(GrupoProdutoModel model);
        Task Atualizar(GrupoProdutoModel model);
    }

    public class GrupoRepository : IGrupoRepository
    {
        private readonly IDapperService _dapper;

        public GrupoRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<GrupoProdutoModel>> Listar(GrupoProdutoModel model)
        {
            try
            {
                model.Descricao = model.Descricao.FormataComoParam();

                string sql = @"
SELECT 
	ID,
	Descricao,
	Observacao
FROM EasyAutoPartsHubDb.dbo.GrupoProduto
WHERE (@ID IS NULL OR ID = @ID)
AND (@Descricao IS NULL OR Descricao LIKE @Descricao)
";

                return await _dapper.QueryAsync<GrupoProdutoModel>(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(GrupoProdutoModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.GrupoProduto
(Descricao, Observacao)
VALUES
(@Descricao, @Observacao)
";

                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryGrupoProduto());
            }
        }

        public async Task Atualizar(GrupoProdutoModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.GrupoProduto SET
Descricao = @Descricao, 
Observacao = @Observacao
WHERE ID = @ID
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryGrupoProduto());
            }
        }
    }
}
