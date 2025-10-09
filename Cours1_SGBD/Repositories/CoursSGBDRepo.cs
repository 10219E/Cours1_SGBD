using Cours1_SGBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging;
using Cours1_SGBD.Interfaces;
using System.Data;

namespace Cours1_SGBD.Repositories
{
    /*public class CoursSGBDRepo
{

    public List<Student> GetStudents()
    {
        List<Student> list = new List<Student>();
        Student student = new Student();
        student.id = "219";
        student.fname = "John";
        student.lname = "Pera";
        list.Add(student);

        return list;
    }
}*/
    public class CoursSGBDRepo : GetFile, ICoursSGBDRepo
    {

        private readonly string _connectionString = GetFileContent("Cours1_SGBD.Repositories.SQL.ConnectionString.txt");

        private readonly ILogger _logger;

        public CoursSGBDRepo(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("SQL_Connection");
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
                //throw;
            }
            
        }


        public List<UI_Student> FindStudentDb(string search)
        {
            var single_student = new List<UI_Student>();
            using (var connection = OpenConnection())
            {
                if (connection == null)
                {
                    return single_student; // Return empty list if connection failed
                }
                else
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }



                string query = GetFileContent("Cours1_SGBD.Repositories.SQL.FIND_STUDENT.sql");
                using (var command = new SqlCommand(query, connection))
                { 
                    if (!int.TryParse(search, out int search_id))
                    {
                        command.Parameters.AddWithValue("@FirstName", "%" + search + "%");
                        command.Parameters.AddWithValue("@LastName", "%" + search + "%");
                        command.Parameters.AddWithValue("@id", 0); //Dummy value, won't be used
                    }
                    else
                    {

                        command.Parameters.AddWithValue("@id", search_id); //using int conversion from the IF
                        command.Parameters.AddWithValue("@FirstName", ""); //Dummy value, won't be used
                        command.Parameters.AddWithValue("@LastName", ""); //Dummy value, won't be used
                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new UI_Student
                            {

                                id = reader.GetInt32(reader.GetOrdinal("ID")),

                                fname = reader.IsDBNull(reader.GetOrdinal("First Name"))
                                     ? throw new Exception($"First name not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString()}")
                                     : reader.GetString(reader.GetOrdinal("First Name")),

                                lname = reader.IsDBNull(reader.GetOrdinal("Last Name"))
                                     ? throw new Exception($"Last name not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString()}")
                                     : reader.GetString(reader.GetOrdinal("Last Name")),

                                //email = reader.GetString(reader.GetOrdinal("E-mail")),
                                //phone = reader.GetString(reader.GetOrdinal("Mobile")),
                                email = reader["E-mail"].ToString(),
                                phone = reader["Mobile"].ToString(),
                                confirmed = reader.GetDateTime(reader.GetOrdinal("Confirmed")),

                                section = reader.IsDBNull(reader.GetOrdinal("Section"))
                                     ? throw new Exception($"Section not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString()}")
                                     : reader.GetString(reader.GetOrdinal("Section"))
                            };
                            single_student.Add(student);
                        }
                        //}
                    }
                }
                return single_student;
            }
        }

        public List<UI_Student> GetStudentsDb()
        {
            var students = new List<UI_Student>();
            using (var connection = OpenConnection())
            {
                if (connection == null)
                {
                        return students; // Return empty list if connection failed
                }
                else
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }



                string query = GetFileContent("Cours1_SGBD.Repositories.SQL.LIST_STUDENTS.sql");
                using (var command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new UI_Student
                        {
                            /*
                            id = (int)reader["ID"],
                            fname = reader["First Name"].ToString(),
                            lname = reader["Last Name"].ToString(),
                            email = reader["E-mail"].ToString(),
                            phone = reader["Mobile"].ToString(),
                            confirmed = Convert.ToDateTime(reader["Confirmed"]),
                            section = reader["Section"] == DBNull.Value ? "" : reader["Section"].ToString()
                            */

                            id = reader.GetInt32(reader.GetOrdinal("ID")),

                            fname = reader.IsDBNull(reader.GetOrdinal("First Name"))
                                 ? throw new Exception($"First name not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString()}")
                                 : reader.GetString(reader.GetOrdinal("First Name")),

                            lname = reader.IsDBNull(reader.GetOrdinal("Last Name"))
                                 ? throw new Exception($"Last name not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString() }")
                                 : reader.GetString(reader.GetOrdinal("Last Name")),

                            //email = reader.GetString(reader.GetOrdinal("E-mail")),
                            //phone = reader.GetString(reader.GetOrdinal("Mobile")),
                            email = reader["E-mail"].ToString(),
                            phone = reader["Mobile"].ToString(),
                            confirmed = reader.GetDateTime(reader.GetOrdinal("Confirmed")),

                            section = reader.IsDBNull(reader.GetOrdinal("Section"))
                                 ? throw new Exception($"Section not specified for ID {(reader.GetInt32(reader.GetOrdinal("ID"))).ToString() }")
                                 : reader.GetString(reader.GetOrdinal("Section"))
                        };
                        students.Add(student);
                    }
                    //}
                }
                return students;
            }
        }

        public void UpdateStudentDb(int id, StudentUpdate update_student)
        {
            using (var connection = OpenConnection())
            {
                if (connection != null)
                    {
                    _logger.LogInformation("Database connection opened successfully.");
                } //No return here as void method


                var updates = new List<string>();
                var command = new SqlCommand();
                command.Connection = connection;

                if (update_student.fname != null)
                {
                    updates.Add("[First Name] = @FirstName");
                    command.Parameters.AddWithValue("@FirstName", update_student.fname);
                }

                if (update_student.lname != null)
                {
                    updates.Add("[Last Name] = @LastName");
                    command.Parameters.AddWithValue("@LastName", update_student.lname);
                }

                if (update_student.email != null)
                {
                    updates.Add("[E-mail] = @Email");
                    command.Parameters.AddWithValue("@Email", update_student.email);
                }

                if (update_student.phone != null)
                {
                    updates.Add("[Mobile] = @Mobile");
                    command.Parameters.AddWithValue("@Mobile", update_student.phone);
                }

                if (update_student.section != null)
                {
                    updates.Add("[Section] = @Section");
                    command.Parameters.AddWithValue("@Section", update_student.section);
                }

                string setClause = string.Join(", ", updates);
                string query = GetFileContent("Cours1_SGBD.Repositories.SQL.UPDATE_STUDENT.sql");
                
                //Appending (by using replace) SetClause in the file string
                string aquery = query.Replace("{SET_CLAUSE}", setClause);   

                //string query = $"UPDATE Students SET {setClause} WHERE ID = @ID";
                command.CommandText = aquery;
                command.Parameters.AddWithValue("@ID", id);

                int rows_affected = command.ExecuteNonQuery();
                if (rows_affected == 0)
                {
                    _logger.LogWarning($"No student found with ID {id} to update.");
                }
                else
                {
                    _logger.LogInformation($"Student with ID {id} updated successfully.");
                }
            }
        }


        public void InsertStudentDb(StudentsToInsert insert_student)
        {
            using (var connection = OpenConnection())
            {
                if (connection != null)
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }//No return here as void method


                string query = GetFileContent("Cours1_SGBD.Repositories.SQL.INSERT_STUDENT.sql");
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", insert_student.fname);
                    command.Parameters.AddWithValue("@LastName", insert_student.lname);
                    command.Parameters.AddWithValue("@Email", insert_student.email);
                    command.Parameters.AddWithValue("@Mobile", insert_student.phone);
                    command.Parameters.AddWithValue("@Confirmed", insert_student.confirmed);
                    command.Parameters.AddWithValue("@Section", insert_student.section);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudentDb(int id)
        {
            using (var connection = OpenConnection())
            {
                if (connection != null)
                {
                    _logger.LogInformation("Database connection opened successfully.");
                }//No return here as void method


                string query = GetFileContent("Cours1_SGBD.Repositories.SQL.DELETE_STUDENT.sql");
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        _logger.LogWarning($"No student found with ID {id} to delete.");
                    }
                    else
                    {
                        _logger.LogInformation($"Student with ID {id} deleted successfully.");
                    }

                }
            }
        }

    }


}
