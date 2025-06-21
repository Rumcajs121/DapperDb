using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MinimalWebApi_net9.Endpoints;

    public class SqlDataAcces(IConfiguration config) : ISqlDataAcces
    {
        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connetionID = "MinimalWebApi")
        {
            using IDbConnection connection = new SqlConnection(config.GetConnectionString(connetionID));

            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connetionID = "MinimalWebApi")
        {
            await using var connection = new SqlConnection(config.GetConnectionString(connetionID));
            await connection.OpenAsync();
            await using var tran = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(storedProcedure, parameters, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Transaction rolled back in procedure '{storedProcedure}'. Error: {e.Message}");
                tran.Rollback();
                throw;
            }
            
        }
    }