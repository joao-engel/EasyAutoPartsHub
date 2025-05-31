using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;

namespace EasyAutoPartsHub.Repository
{
    public interface IPedidoRepository
    {
        Task<List<PedidoStatusModel>> ListarStatus();
		Task<List<PedidoCabecalhoModel>> ListarPedidos(PedidoCabecalhoRQModel model);
		Task<List<PedidoItemModel>> VisualizarPedido(int pedidoID);
    }

    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDapperService _dapper;

        public PedidoRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<PedidoStatusModel>> ListarStatus()
        {
            try
            {
                string sql = @"
SELECT
	ID,
	Nome,
	Ordem
FROM EasyAutoPartsHubDb.dbo.PedidoStatus
";
                return await _dapper.QueryAsync<PedidoStatusModel>(sql, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PedidoCabecalhoModel>> ListarPedidos(PedidoCabecalhoRQModel model)
        {
            try
            {
                string sql = @"
WITH CTE_Pedidos AS
(
	SELECT
		PC.ID,
		PC.ClienteID,
		C.Nome AS Cliente,
		PC.DataEmissao,
		PC.DataFaturamento,
		PC.DataCancelado,
		PC.DataEntregue,
		PC.StatusID,
		PS.Nome AS [Status],
		PC.Observacao	
	FROM EasyAutoPartsHubDb.dbo.PedidoCabecalho PC
	INNER JOIN EasyAutoPartsHubDb.dbo.PedidoStatus PS ON PS.ID = PC.StatusID
	LEFT JOIN EasyAutoPartsHubDb.dbo.Cliente C ON C.ID = PC.ClienteID
	WHERE (@ClienteID IS NULL OR ClienteID = @ClienteID)
	AND (@StatusID IS NULL OR StatusID = @StatusID)
	AND (@ID IS NULL OR PC.ID = @ID)
)
SELECT 
	P.ID,
	P.ClienteID,
	P.Cliente,
	P.DataEmissao,
	P.DataFaturamento,
	P.DataCancelado,
	P.DataEntregue,
	P.StatusID,
	P.[Status],
	SUM(PDI.ValorUnitario * PDI.Quantidade) AS ValorTotal,
	P.Observacao
FROM CTE_Pedidos P
LEFT JOIN EasyAutoPartsHubDb.dbo.PedidoItem PDI ON PDI.PedidoID = P.ID
GROUP BY 
	P.ID,
	P.ClienteID,
	P.Cliente,
	P.DataEmissao,
	P.DataFaturamento,
	P.DataCancelado,
	P.DataEntregue,
	P.StatusID,
	P.[Status],
	P.Observacao
";
				return await _dapper.QueryAsync<PedidoCabecalhoModel>(sql: sql, param: model, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

		public async Task<List<PedidoItemModel>> VisualizarPedido(int pedidoID)
		{
			try
			{
				string sql = @"
SELECT
	P.Descricao AS Produto,
	GP.Descricao AS Grupo,
	PDI.Quantidade,
	PDI.ValorUnitario,
	PDI.Quantidade * PDI.ValorUnitario AS Subtotal
FROM EasyAutoPartsHubDb.dbo.PedidoItem PDI
INNER JOIN EasyAutoPartsHubDb.dbo.Produto P ON P.ID = PDI.ProdutoID
INNER JOIN EasyAutoPartsHubDb.dbo.GrupoProduto GP ON GP.ID = P.GrupoID
WHERE PedidoID = @pedidoID
";
				return await _dapper.QueryAsync<PedidoItemModel>(sql: sql, param: new { pedidoID }, commandType: System.Data.CommandType.Text);
            }
			catch (Exception)
			{
				throw;
			}
		}
    }
}
