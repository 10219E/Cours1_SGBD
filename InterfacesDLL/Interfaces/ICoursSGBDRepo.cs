using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDLL.Models;

namespace InterfacesDLL.Interfaces
{
    public interface ICoursSGBDRepo
    {
        List<ModelsDLL.Models.UI_Student> GetStudentsDb();

        List<ModelsDLL.Models.UI_Student> FindStudentDb(string search);

        void InsertStudentDb(ModelsDLL.Models.StudentsToInsert insert_student);

        void DeleteStudentDb(int id);

        void UpdateStudentDb(int id, ModelsDLL.Models.StudentUpdate student);


    }
}
