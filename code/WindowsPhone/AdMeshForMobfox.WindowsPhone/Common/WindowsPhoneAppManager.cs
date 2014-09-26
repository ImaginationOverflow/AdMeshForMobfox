using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Common;
using AdMesh.Configurator;
using AdMesh.Configurator.WindowsPhone;
using GalaSoft.MvvmLight.Ioc;

namespace AdMeshForMobfox.WindowsPhone.Common
{
    public class WindowsPhoneAppManager : AppManagerBase
    {
        protected override async Task PlatformSetup()
        {
            await base.PlatformSetup();
            SimpleIoc.Default.Register<IAppNavigationService, AppNavService>();
            Configurator.Configure();
        }
    }
}
