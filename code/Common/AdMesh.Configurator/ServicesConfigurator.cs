using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox;
using GalaSoft.MvvmLight.Ioc;

namespace AdMesh.Configurator
{
    static class ServicesConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<IMobfoxService, MobfoxService>();
        }
    }
}
