using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Storage;
using GalaSoft.MvvmLight.Ioc;

namespace AdMesh.Configurator
{
    class StorageConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<MobfoxSessionStorage>();
            SimpleIoc.Default.Register<MobfoxApplicationStorage>();
            SimpleIoc.Default.Register<DashboardStorage>();
        }

    }
}
