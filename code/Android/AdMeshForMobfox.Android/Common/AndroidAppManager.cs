using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Common;
using AdMesh.Configurator;
using AdMesh.Configurator.Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Android.Ui;
using IOToolkit.Helpers;

namespace AdMeshForMobfox.Android.Common
{
    public class AndroidAppManager : AppManagerBase
    {
        private readonly Activity _mainActivity;

        public AndroidAppManager(Activity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        protected override async Task PlatformSetup()
        {
            await base.PlatformSetup();
            SimpleIoc.Default.Register<Context>(() => _mainActivity);
            SimpleIoc.Default.Register(
                () =>
                    new UiResources
                    {
                        AccentColor =
                            "#" + _mainActivity.AppResources.GetColor(Resource.Color.app_accent_color).ToArgb().ToString("X")
                    });
            SimpleIoc.Default.Register<IAppNavigationService>(() => new AppNavigationService(_mainActivity));
            Configurator.Configure();
        }

        public override void PreInitialize()
        {
            base.PreInitialize();
            _mainActivity.SetTheme(Resource.Style.AppTheme);
            _mainActivity.Window.RequestFeature(WindowFeatures.ActionBar);
            _mainActivity.ActionBar.Hide();
        }
    }
}