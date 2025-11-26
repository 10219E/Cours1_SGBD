using InterfacesDLL.Interfaces;
using ModelsDLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesDLL.Services
{
    // Ensure StudioService implements IStudioService
    public class StudioService : IStudioService
    {
        private readonly IStudioRepo _repo;

        public StudioService(IStudioRepo studioRepo)
        {
            _repo = studioRepo;
        }

        public List<UI_StudioStudent> GetStudioSvc()
        {
            List<UI_StudioStudent> studio = _repo.GetStudioDb();
            return studio;
        }

    }
}
