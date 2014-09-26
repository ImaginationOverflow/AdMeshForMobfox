using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdMesh.Common;
using AdMeshForMobfox.WindowsPhone.Views;
using IOToolkit.Navigation;

namespace AdMeshForMobfox.WindowsPhone.Common
{
    class AppNavService : AppNavigationServiceBase
    {
        private Frame Frame { get { return Window.Current.Content as Frame; } }


        public override void GoToLogin()
        {
            Frame.Navigate(typeof (Login));
        }

        public override void GoToHome()
        {
            Frame.Navigate(typeof(Home));

        }

        public override void GoToReportDetails()
        {
            throw new NotImplementedException();
        }

        public override void GoToApplication()
        {
            throw new NotImplementedException();
        }

        public override void GoToCustomReport()
        {
            throw new NotImplementedException();
        }

        public override void GoToAbout()
        {
            throw new NotImplementedException();
        }
    }
}
