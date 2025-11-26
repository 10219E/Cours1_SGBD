using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTEST_Cours1_SGBD.Shared;

namespace xTEST_Cours1_SGBD.Shared
{
    internal class DBSetup : GetFile
    {
        private readonly string _connectionString;

        public DBSetup(string connectionString) : base() => _connectionString = connectionString;

        public async Task InitializeDataBase() => await ExecuteSqlScript("xTEST_Cours1_SGBD.SQL.CreateDatabase.sql");

        public async Task CreateTablesDataBase() => await ExecuteSqlScript("xTEST_Cours1_SGBD.SQL.CreateTables.sql");

        public async Task SeedStudents() => await ExecuteSqlScript("xTEST_Cours1_SGBD.SQL.InitBaseStudents.sql");
        
        public async Task SeedStudio() => await ExecuteSqlScript("xTEST_Cours1_SGBD.SQL.InitBaseStudio.sql");


        public static async Task DropDataBase()
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteSqlScript(string filepath)
        {
            string sql = GetFileContent(filepath);

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection == null)
                {
                    throw new Exception("Failed to open database connection.");
                }
                else
                {
                    int rowsAffected = await connection.ExecuteAsync(sql);
                }
            }
        }
    }
}
