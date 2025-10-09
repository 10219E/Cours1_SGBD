using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cours1_SGBD.Interfaces
{
    public interface ICoursSGBDRepo
    {
        List<Cours1_SGBD.Models.UI_Student> GetStudentsDb();

        List<Cours1_SGBD.Models.UI_Student> FindStudentDb(string search);

        void InsertStudentDb(Cours1_SGBD.Models.StudentsToInsert insert_student);

        void DeleteStudentDb(int id);

        void UpdateStudentDb(int id, Cours1_SGBD.Models.StudentUpdate student);


    }
}
