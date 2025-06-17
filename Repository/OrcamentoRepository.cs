using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;

namespace EasyAutoPartsHub.Repository
{
    public interface IOrcamentoRepository
    {
        Task<List<StatusModel>> ListarStatus();
        Task<List<OrcamentoCabecalhoModel>> ListarOrcamentos(OrcamentoCabecalhoRQModel model);
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
    }
}
