using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDLL.Models;

namespace InterfacesDLL.Interfaces
{
    public interface IStudioRepo
    {
        List<ModelsDLL.Models.UI_StudioStudent> GetStudioDb();

        //List<ModelsDLL.Models.UI_StudioStudent> FindStudioDb(string search);
    }
}
