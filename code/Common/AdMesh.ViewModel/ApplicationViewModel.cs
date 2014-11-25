using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdMesh.Common;
using AdMesh.Controllers;
using AdMesh.Localisation;
using AdMesh.Localisation;
using AdMesh.Service.MobFox.Data;
using AdMesh.ViewModel.Messages;
using IOToolkit.Storage;

namespace AdMesh.ViewModel
{
    public class ApplicationViewModel : AppViewModelBase
    {
        private readonly ReportsController _reportsController;
        private readonly IAppNavigationService _navigationService;

        /// <summary>
        /// The <see cref="CurrentApplication" /> property's name.
        /// </summary>
        public const string CurrentApplicationPropertyName = "CurrentApplication";

        private MobfoxApplication _currentApplication;

        /// <summary>
        /// Sets and gets the CurrentApplication property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxApplication CurrentApplication
        {
            get
            {
                return _currentApplication;
            }

            set
            {
                if (_currentApplication == value)
                {
                    return;
                }

                _currentApplication = value;
                RaisePropertyChanged(CurrentApplicationPropertyName);
            }
        }

        public ApplicationViewModel(ReportsController reportsController, IAppNavigationService navigationService)
        {
            _reportsController = reportsController;
            _navigationService = navigationService;
            Messenger.Register<MobfoxApplication>(this, (a) => CurrentApplication = a);
        }

        private ICommand _navigateToTodayReportCommand;

        /// <summary>
        /// Gets the LoadTodayReportCommand.
        /// </summary>
        public ICommand NavigateToTodayReportCommand
        {
            get
            {
                return _navigateToTodayReportCommand
                    ?? (_navigateToTodayReportCommand = CreateCommandForUserAction<MobfoxApplication>(
                                         async (app) => NavigateToReport(await _reportsController.GetTodayReport(app)), AppResources.App_Loading));

            }
        }

        private ICommand _navigateToOverallReportCommand;

        /// <summary>
        /// Gets the NavigateToOverallReportCommand.
        /// </summary>
        public ICommand NavigateToOverallReportCommand
        {
            get
            {
                return _navigateToOverallReportCommand
                    ?? (_navigateToOverallReportCommand = CreateCommandForUserAction<MobfoxApplication>(
                    async (app) => NavigateToReport(await _reportsController.GetOverallReport(app)), AppResources.App_Loading));

            }
        }

        private ICommand _navigateToYesterdayReportCommand;

        /// <summary>
        /// Gets the NavigateToYesterdayCommand.
        /// </summary>
        public ICommand NavigateToYesterdayCommand
        {
            get
            {
                return _navigateToYesterdayReportCommand
                    ?? (_navigateToYesterdayReportCommand = CreateCommandForUserAction<MobfoxApplication>(
                                                              async (app) => NavigateToReport(await _reportsController.GetYesterdayReport(app)), AppResources.App_Loading));
                ;

            }

        }


        private ICommand _navigateToThisWeekReportCommand;

        /// <summary>
        /// Gets the NavigateToThisWeekReportCommand.
        /// </summary>
        public ICommand NavigateToThisWeekReportCommand
        {
            get
            {
                return _navigateToThisWeekReportCommand
                    ?? (_navigateToThisWeekReportCommand = CreateCommandForUserAction<MobfoxApplication>(
                    async (app) => NavigateToReport(await _reportsController.GetThisWeekReport(app)), AppResources.App_Loading));


            }
        }

        private ICommand _navigateToLastWeekReportCommand;

        /// <summary>
        /// Gets the NavigateToLastWeekReportCommand.
        /// </summary>
        public ICommand NavigateToLastWeekReportCommand
        {
            get
            {
                return _navigateToLastWeekReportCommand
                    ?? (_navigateToLastWeekReportCommand = CreateCommandForUserAction<MobfoxApplication>(
                                         async (app) => NavigateToReport(await _reportsController.GetLastWeekReport(app)), AppResources.App_Loading));

            }
        }

        private ICommand _navigateToThisMonthCommand;

        /// <summary>
        /// Gets the NavigateToThisMonthCommand.
        /// </summary>
        public ICommand NavigateToThisMonthCommand
        {
            get
            {
                return _navigateToThisMonthCommand
                    ?? (_navigateToThisMonthCommand = CreateCommandForUserAction<MobfoxApplication>(
                                    async (app) => NavigateToReport(await _reportsController.GetThisMonthReport(app)), AppResources.App_Loading));

            }
        }

        private ICommand _navigateToLastMonthCommand;

        /// <summary>
        /// Gets the NavigateToLastMonthCommand.
        /// </summary>
        public ICommand NavigateToLastMonthCommand
        {
            get
            {
                return _navigateToLastMonthCommand
                    ?? (_navigateToLastMonthCommand = CreateCommandForUserAction<MobfoxApplication>(
                                          async (app) => NavigateToReport(await _reportsController.GetLastMonthReport(app)), AppResources.App_Loading));
										    
            }
        }


        private void NavigateToReport(MobfoxReport report)
        {
            Messenger.Send(new ReportMessage { Report = report });
            _navigationService.GoToReportDetails();
        }

        protected override void Save(IStorage storage)
        {
            throw new NotImplementedException();
        }

        protected override void Load(IStorage storage)
        {
            throw new NotImplementedException();
        }
    }
}
