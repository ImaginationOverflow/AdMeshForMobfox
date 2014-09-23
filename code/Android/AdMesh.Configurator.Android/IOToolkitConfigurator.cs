using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Android.Services;
using IOToolkit.Android.Storage;
using IOToolkit.Services;
using IOToolkit.Storage;

namespace AdMesh.Configurator.Android
{
    static class IOToolkitConfigurator
    {
        public static void Configure()
        {
            SimpleIoc.Default.Register<ICryptoService, AndroidCryptoService>();
            SimpleIoc.Default.Register<IStorageService, AndroidStorageService>();
            SimpleIoc.Default.Register<IMessageBoxService, AndroidMessageBoxService>();
        }
    }
}