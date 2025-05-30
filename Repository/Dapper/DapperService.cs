using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace EasyAutoPartsHub.Repository.Dapper
{
    public interface IDapperService
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string? nomeTabela = null);
        Task<List<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }

    public class DapperService : IDapperService
    {
        private readonly IDbConnection _conn;

        public DapperService(IConfiguration configuration)
        {
            _conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string? nomeTabela = null)
        {
            try
            {
                var result = await _conn.ExecuteScalarAsync<int>(sql, param, transaction, commandTimeout, commandType);

                return result;
            }
            finally
            {
                _conn.Close();
            }
        }

        public async Task<List<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                var result = await _conn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
                return result.ToList();
            }
            finally
            {
                _conn.Close();
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                return await _conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}
