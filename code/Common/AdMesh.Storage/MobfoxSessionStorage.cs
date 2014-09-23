using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.Service.MobFox.Data;
using IOToolkit.Storage;

namespace AdMesh.Storage
{
    public class MobfoxSessionStorage : BasicStorageService<MobfoxSessionData, MobfoxSessionData>
    {
        public MobfoxSessionStorage(IStorageService storageService) : base(storageService)
        {
        }
    }
}
