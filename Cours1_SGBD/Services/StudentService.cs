using Cours1_SGBD.Models;
using Cours1_SGBD.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cours1_SGBD.Services
{
    public class StudentService
    {
        private readonly CoursSGBDRepo _repo;

        public StudentService()
        {
            _repo = new CoursSGBDRepo(LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("SQL_Exception"));
        }

        public List<Student> GetStudents()
        {
            List<Student> students = _repo.GetStudents();
            return students;
        }
    }
}
