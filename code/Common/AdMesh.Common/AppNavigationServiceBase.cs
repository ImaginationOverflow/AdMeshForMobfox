using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IOToolkit.ObjectModel;

namespace AdMesh.Common
{
    public abstract class AppNavigationServiceBase : IAppNavigationService
    {
        public abstract void GoToLogin();
        public abstract void GoToHome();
        public abstract void GoToReportDetails();
        public abstract void GoToApplication();
        public abstract void GoToCustomReport();
        public abstract void GoToAbout();

        private ICommand _goToLoginCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public ICommand GoToLoginCommand
        {
            get
            {
                return _goToLoginCommand
                    ?? (_goToLoginCommand = new RelayCommand(GoToLogin));
            }
        }

        private ICommand _goToHomeCommand;

        /// <summary>
        /// Gets the GoToHomeCommand.
        /// </summary>
        public ICommand GoToHomeCommand
        {
            get
            {
                return _goToHomeCommand
                    ?? (_goToHomeCommand = new RelayCommand(GoToHome));
            }
        }

        private ICommand _goToReportDetailsCommand;

        /// <summary>
        /// Gets the GoToReportDetailsCommand.
        /// </summary>
        public ICommand GoToReportDetailsCommand
        {
            get
            {
                return _goToReportDetailsCommand
                    ?? (_goToReportDetailsCommand = new RelayCommand(GoToReportDetails));
            }
        }

        private ICommand _goToAppCommand;

        /// <summary>
        /// Gets the GoToApplicationCommand.
        /// </summary>
        public ICommand GoToApplicationCommand
        {
            get
            {
                return _goToAppCommand
                    ?? (_goToAppCommand = new RelayCommand(GoToApplication));
            }
        }

        private ICommand _navToCreateRepoCommand;

        /// <summary>
        /// Gets the GoToCustomReportCommand.
        /// </summary>
        public ICommand GoToCustomReportCommand
        {
            get
            {
                return _navToCreateRepoCommand
                    ?? (_navToCreateRepoCommand = new RelayCommand(GoToCustomReport));
            }
        }

        private ICommand _navigateToAboutCommand;

        /// <summary>
        /// Gets the GoToAboutCommand.
        /// </summary>
        public ICommand GoToAboutCommand
        {
            get
            {
                return _navigateToAboutCommand
                    ?? (_navigateToAboutCommand = new RelayCommand(GoToAbout));
            }
        }
    }
}
