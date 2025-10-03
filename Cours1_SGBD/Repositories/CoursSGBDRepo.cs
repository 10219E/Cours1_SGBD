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
    public class CoursSGBDRepo : ICoursSGBDRepo
    {
        private readonly string _connectionString = "Server=CUTE59\\SQL_EPS;Database=SGBD_C;User Id=PRO;Password=;TrustServerCertificate=True";

        private readonly ILogger _logger;

        public CoursSGBDRepo(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("SQL_Connection");
        }

        public void InsertStudentDb(StudentsToInsert insert_student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Students ([First Name], [Last Name], [E-mail], [Mobile], [Confirmed], [Section]) " +
                               "VALUES (@FirstName, @LastName, @Email, @Mobile, @Confirmed, @Section)";
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
                string query = "DELETE FROM Students WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
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


                string query = "SELECT * FROM Students";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            id = reader["ID"].ToString(),
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
    }


}
