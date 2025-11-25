using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDLL.Models;

namespace InterfacesDLL.Interfaces
{
    public interface IStudentService
    {
        List<ModelsDLL.Models.UI_Student> GetStudentsSvc();

        List<ModelsDLL.Models.UI_Student> FindStudentSvc(string search);

        List<ModelsDLL.Models.UI_StudioStudent> GetStudioSvc();

        void UpdateStudentSvc(int id, ModelsDLL.Models.StudentUpdate updatedStudent);

        void InsertStudentSvc(ModelsDLL.Models.StudentsToInsert insert_student);

        void DeleteStudentSvc(int id);
    }
}
