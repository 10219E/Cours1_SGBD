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

        public CoursSGBDRepo(ILogger<CoursSGBDRepo> logger)
        {
            _logger = logger;
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
