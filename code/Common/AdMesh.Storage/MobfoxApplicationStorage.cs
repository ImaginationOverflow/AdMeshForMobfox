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
    
    public class MobfoxApplicationStorage : BasicCollectionStorageService<MobfoxApplication, MobfoxApplication>
    {
        public MobfoxApplicationStorage(IStorageService storageService) : base(storageService)
        {
        }
    }
}
