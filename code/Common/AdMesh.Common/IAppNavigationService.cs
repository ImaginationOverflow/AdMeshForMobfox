using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdMesh.Common
{
    public interface IAppNavigationService
    {
        void GoToLogin();
        void GoToHome();
        void GoToReportDetails();
        void GoToApplication();
        void GoToCustomReport();
        void GoToAbout();


        ICommand GoToLoginCommand { get; }
        ICommand GoToHomeCommand { get; }
        ICommand GoToReportDetailsCommand { get; }
        ICommand GoToApplicationCommand { get; }
        ICommand GoToCustomReportCommand { get; }
        ICommand GoToAboutCommand { get; }
    }
}
