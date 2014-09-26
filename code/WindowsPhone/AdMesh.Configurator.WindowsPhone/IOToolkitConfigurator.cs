using GalaSoft.MvvmLight.Ioc;
using IOToolkit;
using IOToolkit.Navigation;
using IOToolkit.Services;
using IOToolkit.Storage;
using IOToolkit.WinRt;
using IOToolkit.WinRt.Services;
using IOToolkit.WinRt.Storage;

namespace AdMesh.Configurator.WindowsPhone
{
    static class IOToolkitConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<ICryptoService, WinRtCryptoService>();
            SimpleIoc.Default.Register<IStorageService, ApplicationDataStorage>();
            SimpleIoc.Default.Register<IMessageBoxService, WinRtMessageBoxService>();
            SimpleIoc.Default.Register<IExecutionContext, WinRtDispatcherContext>();
        }
    }
}