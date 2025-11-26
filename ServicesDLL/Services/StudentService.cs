using ModelsDLL.Models;
using InterfacesDLL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesDLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly ICoursSGBDRepo _repo;
        private readonly IStudioRepo _studioRepo;

        public StudentService(ICoursSGBDRepo coursSGBDRepo, IStudioRepo studioRepo)
        {
            _repo = coursSGBDRepo;
            _studioRepo = studioRepo;
        }

        public List<UI_Student> FindStudentSvc(string search)
        {
            List<UI_Student> student = _repo.FindStudentDb(search);
            return student;
        }

        public List<UI_Student> GetStudentsSvc()
        {
            List<UI_Student> students = _repo.GetStudentsDb();
            return students;
        }

        public List<UI_StudioStudent> GetStudioSvc()
        {
            List<UI_StudioStudent> studio = _studioRepo.GetStudioDb();
            return studio;
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
