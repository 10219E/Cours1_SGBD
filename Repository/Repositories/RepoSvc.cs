using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging;
using System.Data;
using InterfacesDLL.Interfaces;
using ModelsDLL.Models;
using RepositoryDLL.FileSvc;
using Dapper;

namespace RepositoryDLL.RepoSvc
{

    public class RepoSvc : GetFile, ICoursSGBDRepo
    {

        private readonly string _connectionString = GetFileContent("RepositoryDLL.Repositories.SQL.ConnectionString.txt");

        private readonly ILogger _logger;

        public RepoSvc(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("SQL_Connection");
        }

        public RepoSvc(ILoggerFactory logger, string connectionString)
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

        public List<UI_Student> GetStudentsDb() { 

            var students = new List<UI_Student>();
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.LIST_STUDENTS.sql");

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    return students; // Return empty list if connection failed
                }
                else
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }

                students = connection.Query<UI_Student>(query).ToList();
            }

            return students;

        }


        public List<UI_Student> FindStudentDb(string search)
        {
            var single_student = new List<UI_Student>();
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.FIND_STUDENT.sql");

            Dictionary<string, object> dbArgs = new Dictionary<string, object>();
            dbArgs.Add("@search", search);

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    return single_student; // Return empty list if connection failed
                }
                else
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }

                single_student = connection.Query<UI_Student>(query, dbArgs).ToList();
            }

            return single_student;

        }

        public void UpdateStudentDb(int id, StudentUpdate update_student)
        {
            string queryTmp = GetFileContent("RepositoryDLL.Repositories.SQL.UPDATE_STUDENT.sql");

            // single dictionary keyed by SQL column names (brackets preserved)
            var updates = new Dictionary<string, object?>()
            {
                ["[First Name]"] = update_student.fname,
                ["[Last Name]"] = update_student.lname,
                ["[E-mail]"] = update_student.email,
                ["[Mobile]"] = update_student.phone,
                ["[Section]"] = update_student.section
            };

            var setParts = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var kvp in updates)
            {
                if (kvp.Value == null) // skip null => do not update this column
                    continue;

                // create a safe parameter name from the column name (strip brackets, spaces, dashes) ex: [First Name] --> FirstName
                var paramName = kvp.Key.Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "");

                setParts.Add($"{kvp.Key} = @{paramName}");
                parameters.Add(paramName, kvp.Value);
            }

            if (setParts.Count == 0)
            {
                _logger.LogInformation($"No fields to update for ID {id}.");
                return;
            }

            var setClause = string.Join(", ", setParts);
            var query = queryTmp.Replace("{SET_CLAUSE}", setClause);
            parameters.Add("ID", id);

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    _logger.LogError("Database connection failed. Update aborted.");
                    return;
                }
                _logger.LogInformation("Database connection opened successfully.");

                int rows = connection.Execute(query, parameters);

                if (rows == 0)
                    _logger.LogWarning($"No student found with ID {id} to update.");
                else
                    _logger.LogInformation($"Student with ID {id} updated successfully. Rows affected: {rows}.");
            }
        }

        public void InsertStudentDb(StudentsToInsert insert_student)
        {
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.INSERT_STUDENT.sql");

            var insert = new Dictionary<string, object?>()
            {
                ["@FirstName"] = insert_student.fname,
                ["@LastName"] = insert_student.lname,
                ["@Email"] = insert_student.email,
                ["@Mobile"] = insert_student.phone,
                ["@Confirmed"] = insert_student.confirmed,
                ["@Section"] = insert_student.section
            };

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    _logger.LogError("Database connection failed. Insert aborted.");
                    return;
                }
                _logger.LogInformation("Database connection opened successfully.");
                
                int rows = connection.Execute(query, insert);
                
                if (rows == 0)
                    _logger.LogWarning("Insert operation did not affect any rows.");
                else
                    _logger.LogInformation($"Student inserted successfully. Rows affected: {rows}.");
            }
        }

        public void DeleteStudentDb(int id)
        {
            string query = GetFileContent("RepositoryDLL.Repositories.SQL.DELETE_STUDENT.sql");

            using (IDbConnection connection = OpenConnection())
            {
                if (connection == null)
                {
                    _logger.LogError("Database connection failed. Delete aborted.");
                    return;
                }
                _logger.LogInformation("Database connection opened successfully.");
                var parameters = new { ID = id };

                int rows = connection.Execute(query, parameters);

                if (rows == 0)
                    _logger.LogWarning($"No student found with ID {id} to delete.");
                else
                    _logger.LogInformation($"Student with ID {id} deleted successfully. Rows affected: {rows}.");
            }
        }


    }


}
