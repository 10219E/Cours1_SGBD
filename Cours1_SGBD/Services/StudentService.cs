using Cours1_SGBD.Interfaces;
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
    public class StudentService : IStudentService
    {
        private readonly CoursSGBDRepo _repo;

        public StudentService(ILogger<StudentService> logger, ICoursSGBDRepo coursSGBDRepo)
        {
            _repo = (CoursSGBDRepo?)coursSGBDRepo;
        }

        public List<Student> GetStudents()
        {
            List<Student> students = _repo.GetStudents();
            return students;
        }
    }
}
