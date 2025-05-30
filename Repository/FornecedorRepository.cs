using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Repository.Dapper;
using EasyAutoPartsHub.Repository.ExceptionCustom;
using System.Data;

namespace EasyAutoPartsHub.Repository
{
    public interface IFornecedorRepository
    {
        Task<List<FornecedorModel>> Listar(FornecedorModel model);
        Task Inserir(FornecedorModel model);
        Task Atualizar(FornecedorModel model);
    }
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly IDapperService _dapper;

        public FornecedorRepository(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<FornecedorModel>> Listar(FornecedorModel model)
        {
            try
            {
                model.NomeFantasia = model.NomeFantasia.FormataComoParam();
                model.RazaoSocial = model.RazaoSocial.FormataComoParam();
                model.CNPJ = model.CNPJ.FormataComoParam();
                model.Telefone = model.Telefone.FormataTelComoParam();

                string sql = @"
SELECT 
	ID,
	NomeFantasia,
    RazaoSocial,
	CNPJ,
	Telefone
FROM EasyAutoPartsHubDb.dbo.Fornecedor
WHERE (@ID IS NULL OR ID = @ID)
AND (@NomeFantasia IS NULL OR NomeFantasia = @NomeFantasia)
AND (@RazaoSocial IS NULL OR RazaoSocial = @RazaoSocial)
AND (@CNPJ IS NULL OR CNPJ = @CNPJ)
AND (@Telefone IS NULL OR Telefone = @Telefone)
";

                return await _dapper.QueryAsync<FornecedorModel>(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(FornecedorModel model)
        {
            try
            {
                string sql = @"
INSERT INTO EasyAutoPartsHubDb.dbo.Fornecedor
(NomeFantasia,
RazaoSocial,
CNPJ,
Telefone)
VALUES
(@NomeFantasia,
@RazaoSocial,
@CNPJ,
@Telefone)
";

                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryFornecedor());
            }
        }

        public async Task Atualizar(FornecedorModel model)
        {
            try
            {
                string sql = @"
UPDATE EasyAutoPartsHubDb.dbo.Fornecedor SET
NomeFantasia = @NomeFantasia,
RazaoSocial = @RazaoSocial,
CNPJ = @CNPJ,
Telefone @Telefone
WHERE ID = @ID
";

                await _dapper.ExecuteAsync(sql: sql, param: model, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ExceptionRepositoryFornecedor());
            }
        }
    }
}
