using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace AdMesh.Configurator
{
    public class CommonConfigurator
    {
        public static void Configure()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            ServicesConfigurator.Configure();
            IOToolkitConfigurator.Configure();
            ViewModelManager.ConfigureViewModels();
            ExceptionHandlerConfigurator.Configure();
            StorageConfigurator.Configure();
            ControllersConfigurator.Configure();
        }
    }
}
