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

        public StudentService(ICoursSGBDRepo coursSGBDRepo)
        {
            _repo = (CoursSGBDRepo?)coursSGBDRepo;
        }

        public List<Student> GetStudentsSvc()
        {
            List<Student> students = _repo.GetStudentsDb();
            return students;
        }
        
        public void UpdateStudentSvc(int id, StudentUpdate updatedStudent)
        {
            var studentToUpdate = new StudentUpdate
            {
                fname = updatedStudent.fname,
                lname = updatedStudent.lname,
                email = updatedStudent.email,
                phone = updatedStudent.phone,
                section = updatedStudent.section
            };
            _repo.UpdateStudentDb(id, studentToUpdate);
        }

        public void InsertStudentSvc(StudentsToInsert insertStudent)
        {
            // You must map this to a real Student model if your DB expects it
            var insert_student = new StudentsToInsert
            {
                fname = insertStudent.fname,
                lname = insertStudent.lname,
                email = insertStudent.email,
                phone = insertStudent.phone,
                confirmed = insertStudent.confirmed,
                section = insertStudent.section
            };

            _repo.InsertStudentDb(insert_student);
        }


        public void DeleteStudentSvc(int id)
        {
            _repo.DeleteStudentDb(id);
        }
    }
}
