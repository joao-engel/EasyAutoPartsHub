using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using System.Data;

namespace EasyAutoPartsHub.Repository;

public interface IRelatorioRepository
{
    Task<List<RelFaturamentoProdutoModel>> FaturamentoProduto(DateTime dataIni, DateTime dataFim);
    Task<List<RelFaturamentoClienteModel>> FaturamentoCliente(DateTime dataIni, DateTime dataFim);
    Task<List<RelOrcamentoStatusModel>> OrcamentoStatus(DateTime dataIni, DateTime dataFim);
}

public class RelatorioRepository : IRelatorioRepository
{
    private readonly IDapperService _dapper;

    public RelatorioRepository(IDapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<List<RelFaturamentoProdutoModel>> FaturamentoProduto(DateTime dataIni, DateTime dataFim)
    {
        try
        {
            string sql = @"
;WITH CTE_Pedidos AS
(
	SELECT
		ID
	FROM EasyAutoPartsHubDb.dbo.PedidoCabecalho PC
	WHERE PC.DataFaturamento BETWEEN @dataIni AND @dataFim
AND PC.StatusID IN (2,3)
)
SELECT
	P.Descricao AS Produto,
	SUM(PDI.Quantidade) AS Quantidade,
	SUM(PDI.Quantidade * PDI.ValorUnitario) AS ValorFaturado
FROM CTE_Pedidos PED
INNER JOIN EasyAutoPartsHubDb.dbo.PedidoItem PDI ON PDI.PedidoID = PED.ID
LEFT JOIN EasyAutoPartsHubDb.dbo.Produto P ON P.ID = PDI.ProdutoID
GROUP BY P.Descricao
";
            return await _dapper.QueryAsync<RelFaturamentoProdutoModel>(sql: sql, param: new { dataIni, dataFim }, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<RelFaturamentoClienteModel>> FaturamentoCliente(DateTime dataIni, DateTime dataFim)
    {
        try
        {
            string sql = @"
;WITH CTE_Pedidos AS
(
	SELECT
		PC.ID,
		C.Nome
	FROM EasyAutoPartsHubDb.dbo.PedidoCabecalho PC
	INNER JOIN EasyAutoPartsHubDb.dbo.Cliente C ON C.ID = PC.ClienteID
	WHERE PC.DataFaturamento BETWEEN @dataIni AND @dataFim
	AND PC.StatusID IN (2,3)
)
SELECT
	PED.Nome AS Cliente,
	SUM(PDI.Quantidade) AS Quantidade,
	SUM(PDI.Quantidade * PDI.ValorUnitario) AS ValorFaturado
FROM CTE_Pedidos PED
INNER JOIN EasyAutoPartsHubDb.dbo.PedidoItem PDI ON PDI.PedidoID = PED.ID
GROUP BY PED.Nome
";
            return await _dapper.QueryAsync<RelFaturamentoClienteModel>(sql: sql, param: new { dataIni, dataFim }, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<List<RelOrcamentoStatusModel>> OrcamentoStatus(DateTime dataIni, DateTime dataFim)
    {
        try
        {
            string sql = @"
;WITH CTE_Orcamentos AS
(
	SELECT 
		OC.ID AS OrcamentoID,
		OS.Nome AS [Status]
	FROM EasyAutoPartsHubDb.dbo.OrcamentoCabecalho OC
	INNER JOIN EasyAutoPartsHubDb.dbo.OrcamentoStatus OS ON OS.ID = OC.StatusID
	WHERE DataOrcamento BETWEEN @dataIni AND @dataFim
)
SELECT 
	O.[Status],
	COUNT(DISTINCT OI.OrcamentoID) AS QuantidadeOrcamentos,
	SUM(OI.Quantidade) AS QuantidadeProdutos,
	SUM(OI.Quantidade * OI.ValorUnitario) AS ValorTotal
FROM CTE_Orcamentos AS O
INNER JOIN EasyAutoPartsHubDb.dbo.OrcamentoItem OI ON OI.OrcamentoID = O.OrcamentoID
GROUP BY O.[Status]
";
            return _dapper.QueryAsync<RelOrcamentoStatusModel>(sql: sql, param: new { dataIni, dataFim }, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
