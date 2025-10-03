using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cours1_SGBD.Interfaces
{
    public interface ICoursSGBDRepo
    {
        List<Cours1_SGBD.Models.Student> GetStudentsDb();

        void InsertStudentDb(Cours1_SGBD.Models.StudentsToInsert insert_student);

        void DeleteStudentDb(int id);
    }
}
