using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using System.Data;

namespace EasyAutoPartsHub.Repository;

public interface IRelatorioRepository
{
    Task<List<RelFaturamentoProdutoModel>> FaturamentoPeriodo(DateTime dataIni, DateTime dataFim);
}

public class RelatorioRepository : IRelatorioRepository
{
    private readonly IDapperService _dapper;

    public RelatorioRepository(IDapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<List<RelFaturamentoProdutoModel>> FaturamentoPeriodo(DateTime dataIni, DateTime dataFim)
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
}
