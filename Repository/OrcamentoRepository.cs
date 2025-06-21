using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using System.Data;

namespace EasyAutoPartsHub.Repository
{
    public interface IOrcamentoRepository
    {
        Task<List<StatusModel>> ListarStatus();
        Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model);
        Task<List<OrcamentoItemModel>> ListarItensPorOrcamento(int id);
        Task<int> InserirOrcamentoCabecalho(OrcamentoCabecalhoModel model);
        Task AtualizarOrcamentoCabecalho(OrcamentoCabecalhoModel model);
        Task InserirOrcamentoItem(OrcamentoItemModel model);
        Task DeletarItensPorOrcamento(int orcamentoID);
        Task DescartarOrcamento(int id);
        Task ConcluirOrcamento(int id);
    }

    public class OrcamentoRepository : IOrcamentoRepository
    {
        private readonly IDapperService _dapper;

        public OrcamentoRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<StatusModel>> ListarStatus()
        {
            try
            {
                string sql = @"
SELECT
	ID,
	Nome,
	Ordem
FROM EasyAutoPartsHubDb.dbo.OrcamentoStatus
";
                return await _dapper.QueryAsync<StatusModel>(sql, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model)
        {
            try
            {
                string sql = @"
WITH CTE_Orcamentos AS
(
	SELECT
		OC.ID,
		OC.ClienteID,
		C.Nome AS Cliente,
		OC.DataOrcamento,
		OC.StatusID,
		OS.Nome AS [Status],
		OC.Observacao
	FROM EasyAutoPartsHubDb.dbo.OrcamentoCabecalho OC
	INNER JOIN EasyAutoPartsHubDb.dbo.Cliente C ON C.ID = OC.ClienteID
	INNER JOIN EasyAutoPartsHubDb.dbo.OrcamentoStatus OS ON OS.ID = OC.StatusID
	WHERE (@ID IS NULL OR OC.ID = @ID)
	AND (@ClienteID IS NULL OR OC.ClienteID = @ClienteID)
	AND (@StatusID IS NULL OR OC.StatusID = @StatusID)
)
SELECT
	O.ID,
	O.ClienteID,
	O.Cliente,
	O.DataOrcamento,
	O.StatusID,
	O.[Status],
	SUM(OI.ValorUnitario * OI.Quantidade) AS ValorTotal,
	SUM(OI.Quantidade) AS QuantidadeItens,
	O.Observacao
FROM CTE_Orcamentos O
LEFT JOIN EasyAutoPartsHubDb.dbo.OrcamentoItem OI ON OI.OrcamentoID = O.ID
GROUP BY
	O.ID,
	O.ClienteID,
	O.Cliente,
	O.DataOrcamento,
	O.StatusID,
	O.[Status],
	O.Observacao
";
                return await _dapper.QueryAsync<OrcamentoCabecalhoModel>(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OrcamentoItemModel>> ListarItensPorOrcamento(int id)
        {
            try
            {
                string sql = @"
SELECT
	OI.OrcamentoID,
	OI.ProdutoID,
	P.Descricao AS Produto,
	GP.Descricao AS Grupo,
	OI.Quantidade,
	OI.ValorUnitario,
	SUM(OI.Quantidade) * OI.ValorUnitario AS SubTotal
FROM EasyAutoPartsHubDb.dbo.OrcamentoItem OI
INNER JOIN EasyAutoPartsHubDb.dbo.Produto P ON P.ID = OI.ProdutoID
INNER JOIN EasyAutoPartsHubDb.dbo.GrupoProduto GP ON GP.ID = P.GrupoID
WHERE OrcamentoID = @id
GROUP BY
	OI.OrcamentoID,
	OI.ProdutoID,
	P.Descricao,
	GP.Descricao,
	OI.Quantidade,
	OI.ValorUnitario
";
                return await _dapper.QueryAsync<OrcamentoItemModel>(sql: sql, param: new { id }, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InserirOrcamentoCabecalho(OrcamentoCabecalhoModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.OrcamentoCabecalho
(ClienteID, DataOrcamento, StatusID, Observacao)
VALUES
(@ClienteID, @DataOrcamento, @StatusID, @Observacao)
SELECT SCOPE_IDENTITY();
";
                int ret = await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AtualizarOrcamentoCabecalho(OrcamentoCabecalhoModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.OrcamentoCabecalho SET
ClienteID = @ClienteID,
DataOrcamento = @DataOrcamento,
StatusID = @StatusID,
Observacao = @Observacao
WHERE ID = @ID
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InserirOrcamentoItem(OrcamentoItemModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.OrcamentoItem
(OrcamentoID, ProdutoID, Quantidade, ValorUnitario)
VALUES
(@OrcamentoID, @ProdutoID, @Quantidade, @ValorUnitario)
";
                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletarItensPorOrcamento(int orcamentoID)
        {
            try
            {
                string sql = @"
DELETE FROM EasyAutoPartsHubDb.dbo.OrcamentoItem 
WHERE OrcamentoID = @orcamentoID
";
                await _dapper.ExecuteAsync(sql: sql, param: new { orcamentoID }, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DescartarOrcamento(int id)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.OrcamentoCabecalho SET
StatusID = 3
WHERE ID = @id
";
                await _dapper.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ConcluirOrcamento(int id)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.OrcamentoCabecalho SET
StatusID = 2
WHERE ID = @id
";
                await _dapper.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
