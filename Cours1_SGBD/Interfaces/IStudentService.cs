using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cours1_SGBD.Interfaces
{
    public interface IStudentService
    {
        List<Cours1_SGBD.Models.Student> GetStudentsSvc();

        void UpdateStudentSvc(int id, Cours1_SGBD.Models.StudentUpdate updatedStudent);

        void InsertStudentSvc(Cours1_SGBD.Models.StudentsToInsert insert_student);

        void DeleteStudentSvc(int id);
    }
}
