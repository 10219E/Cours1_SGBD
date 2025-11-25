using InterfacesDLL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ModelsDLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryDLL.FileSvc;
using Dapper;

namespace RepositoryDLL.RepoSvc
{
    public class StudioRepoSvc : GetFile, IStudioRepo
    {
        private readonly string _connectionString = GetFileContent("RepositoryDLL.Repositories.SQL.ConnectionString.txt");

        private readonly ILogger _logger;

        public StudioRepoSvc(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("SQL_Connection");
        }

        public StudioRepoSvc(ILoggerFactory logger, string connectionString)
        {
            _logger = logger.CreateLogger("SQL_Connection");
            _connectionString = connectionString;
        }

        private SqlConnection OpenConnection() //Opening connection in a separate method to avoid code duplication
        {
            var connection = new SqlConnection(_connectionString);

            try
            {
                _logger.LogInformation("Attempting to open database connection.");
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while trying to open database connection.");
                connection.Dispose();
                return null;
            }

        }

        public List<UI_StudioStudent> GetStudioDb()
        {
            var studios = new List<UI_StudioStudent>();
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.LIST_STUDIO.sql");
            
            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    return studios;
                }
                
                _logger.LogInformation("Database connection opened successfully.");
                
                studios = connection.Query<UI_StudioStudent, UI_Student, UI_StudioStudent>(
                    query,
                    (studio, student) =>
                    {
                        studio.Student = student;
                        return studio;
                    },
                    splitOn: "id"  // Tells Dapper that columns from "id" onwards belong to UI_Student
                ).ToList();
            }
            
            return studios;

        }


        public List<UI_StudioStudent> FindStudioDb(string search)
        {
            var single_studio = new List<UI_StudioStudent>();
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.FIND_STUDENT.sql");

            Dictionary<string, object> dbArgs = new Dictionary<string, object>();
            dbArgs.Add("@search", search);

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    return single_studio; // Return empty list if connection failed
                }
                else
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }

                single_studio = connection.Query<UI_StudioStudent>(query, dbArgs).ToList();
            }

            return single_studio;

        }

    }
}
