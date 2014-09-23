using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Controllers.Data;
using AdMesh.Service.MobFox.Data;
using IOToolkit.Storage;

namespace AdMesh.Storage
{
    public class DashboardStorage : BasicStorageService<DashboardInfo, DashboardInfo>
    {
        public DashboardStorage(IStorageService storageService) : base(storageService)
        {
        }
    }
}
