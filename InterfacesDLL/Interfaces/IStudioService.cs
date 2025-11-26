using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDLL.Models;

namespace InterfacesDLL.Interfaces
{
    public interface IStudioService
    {
        List<ModelsDLL.Models.UI_StudioStudent> GetStudioSvc();
    }
}
