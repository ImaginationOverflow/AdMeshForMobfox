using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Controllers;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Controllers;

namespace AdMesh.Configurator
{
    class ControllersConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<AccountController>();
            SimpleIoc.Default.Register<ReportsController>();
            SimpleIoc.Default.Register<ApplicationsController>();

            var controllers = new ControllerCollection
                (
                SimpleIoc.Default.GetInstance<AccountController>,
                SimpleIoc.Default.GetInstance<ApplicationsController>,
                SimpleIoc.Default.GetInstance<ReportsController>
                );

            SimpleIoc.Default.Register(() => controllers);
        }
    }
}
