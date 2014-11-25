using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Localisation;
using AdMesh.Localisation;
using AdMesh.Service.MobFox.Error;
using IOToolkit.Services;

namespace AdMesh.Configurator
{
    public class ExceptionHandlerConfigurator
    {
        public static void Configure()
        {
            ExceptionHandlerService.Instance.Reset();
#if DEBUG
            ExceptionHandlerService.Instance.SetDefaultExceptionHandler((e) => e.ToString());
#else
            ExceptionHandlerService.Instance.SetDefaultExceptionHandler((e) => AppResources.Error_UnknownError);
            ExceptionHandlerService.Instance.AddHandler(typeof(WebException), e => AppResources.Error_NoConnection);
            ExceptionHandlerService.Instance.AddHandler(typeof(MobfoxNoReportException), e => AppResources.Error_NoReport);
#endif
        }
    }
}
