using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Messaging;
using IOToolkit.Services;
using IOToolkit.Storage;

namespace AdMesh.Configurator
{
    public class IOToolkitConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<IMessenger, LastMessageReplayMessenger>();

            AppCommonServicesContainer.Instance.MessageBoxServiceFactory = SimpleIoc.Default.GetInstance<IMessageBoxService>;
            AppCommonServicesContainer.Instance.MessengerFactory = SimpleIoc.Default.GetInstance<IMessenger>;
            AppCommonServicesContainer.Instance.StorageServiceFactory = SimpleIoc.Default.GetInstance<IStorageService>;
        }
    }
}
