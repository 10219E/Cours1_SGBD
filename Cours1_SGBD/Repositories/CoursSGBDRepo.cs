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

        private readonly string _connectionString = GetFileContent("Cours1_SGBD.Repositories.ConnectionString.txt");

        private readonly ILogger _logger;

        public CoursSGBDRepo(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("SQL_Connection");
        }


        public List<Student> GetStudentsDb()
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try {                     
                    _logger.LogInformation("Attempting to open database connection.");
                    connection.Open();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while trying to open database connection.");
                    //throw;
                    return students;
                }

                string query = GetFileContent("Cours1_SGBD.Repositories.LIST_STUDENTS.sql");
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            id = (int)reader["ID"],
                            fname = reader["First Name"].ToString(),
                            lname = reader["Last Name"].ToString(),
                            email = reader["E-mail"].ToString(),
                            phone = reader["Mobile"].ToString(),
                            confirmed = Convert.ToDateTime(reader["Confirmed"]),
                            section = reader["Section"].ToString()
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }

        public void UpdateStudentDb(int id, StudentUpdate update_student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

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
                string query = GetFileContent("Cours1_SGBD.Repositories.UPDATE_STUDENT.sql");
                
                //Appending (by using replace) SetClause in the file string
                string aquery = query.Replace("{SET_CLAUSE}", setClause);   

                //string query = $"UPDATE Students SET {setClause} WHERE ID = @ID";
                command.CommandText = aquery;
                command.Parameters.AddWithValue("@ID", id);

                command.ExecuteNonQuery();
            }
        }


        public void InsertStudentDb(StudentsToInsert insert_student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                string query = GetFileContent("Cours1_SGBD.Repositories.INSERT_STUDENT.sql");
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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = GetFileContent("Cours1_SGBD.Repositories.DELETE_STUDENT.sql");
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
        }

    }


}
