using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using EasyAutoPartsHub.Repository.ExceptionCustom;

namespace EasyAutoPartsHub.Repository
{
    public interface IProdutoRepository
    {
        Task<List<ProdutoModel>> Listar(ProdutoRQModel model);
        Task Inserir(ProdutoModel model);
        Task Atualizar(ProdutoModel model);
    }

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDapperService _dapper;

        public ProdutoRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<ProdutoModel>> Listar(ProdutoRQModel model)
        {
            try
            {
                model.CodigoExterno = model.CodigoExterno.FormataComoParam();
                model.Descricao = model.Descricao.FormataComoParam();

                string sql = @"
SELECT 
	P.ID,
	P.CodigoExterno,
	P.Descricao,
	P.GrupoID,
	GP.Descricao AS Grupo,
	P.FornecedorID,
	F.NomeFantasia AS Fornecedor,
	P.Preco,
    P.Ativo
FROM EasyAutoPartsHubDb.dbo.Produto P
INNER JOIN EasyAutoPartsHubDb.dbo.GrupoProduto GP ON GP.ID = P.GrupoID
INNER JOIN EasyAutoPartsHubDb.dbo.Fornecedor F ON F.ID = P.FornecedorID
WHERE (@ID IS NULL OR P.ID = @ID)
AND (@CodigoExterno IS NULL OR P.CodigoExterno LIKE @CodigoExterno)
AND (@Descricao IS NULL OR P.Descricao LIKE @Descricao)
AND (@GrupoID IS NULL OR P.GrupoID = @GrupoID)
AND (@FornecedorID IS NULL OR P.FornecedorID = @FornecedorID)
AND (@Ativo IS NULL OR P.Ativo = @Ativo)
";
                return await _dapper.QueryAsync<ProdutoModel>(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(ProdutoModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.Produto
(CodigoExterno, 
Descricao, 
GrupoID,
FornecedorID, 
Preco,
Ativo)
VALUES
(@CodigoExterno, 
@Descricao, 
@GrupoID,
@FornecedorID, 
@Preco,
@Ativo)";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryProduto());
            }
        }

        public async Task Atualizar(ProdutoModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.Produto SET
CodigoExterno = @CodigoExterno, 
Descricao = @Descricao, 
GrupoID = @GrupoID,
FornecedorID = @FornecedorID, 
Preco = @Preco,
Ativo = @Ativo
WHERE ID = @ID
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryProduto());
            }
        }
    }
}
