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
using IOToolkit.Helpers;
using IOToolkit.Storage;

namespace AdMesh.ViewModel
{
    public class HomeViewModel : AppViewModelBase
    {
        public IAppNavigationService AppNavigationService { get; private set; }
        public ReportsController ReportsController { get; private set; }
        public ApplicationsController ApplicationsController { get; private set; }



        public HomeViewModel(ReportsController reportsController, ApplicationsController applicationsController, IAppNavigationService appNavigationService)
        {
            AppNavigationService = appNavigationService;
            ReportsController = reportsController;
            ApplicationsController = applicationsController;
        }

        private ICommand _refreshCommand;

        /// <summary>
        /// Gets the LoadDataCommand.
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = CreateCommand(
                        () =>
                        {

                            var dashboardTask = ReportsController.RefreshDashboardAsync();
                            var appsTask = ApplicationsController.LoadApplicationsAsync();

                            return Task.WhenAll(appsTask, dashboardTask);
                        }, AppResources.Home_LoadingDashboard));

            }
        }

        private ICommand _goToReportCommand;

        /// <summary>
        /// Gets the ShowReportDetailsCommand.
        /// </summary>
        public ICommand ShowReportDetailsCommand
        {
            get
            {
                return _goToReportCommand
                    ?? (_goToReportCommand = CreateCommandForUserAction<MobfoxReport>(
                                          (r) =>
                                          {
                                              Messenger.Send(new ReportMessage { Report = r });
                                              AppNavigationService.GoToReportDetails();
                                              return TaskHelpers.FinishTask();
                                          }, AppResources.App_Loading));

            }
        }

        private ICommand _navigateToApplicationCommand;

        /// <summary>
        /// Gets the NavigateToApplicationCommand.
        /// </summary>
        public ICommand NavigateToApplicationCommand
        {
            get
            {
                return _navigateToApplicationCommand
                    ?? (_navigateToApplicationCommand = CreateCommandForUserAction<MobfoxApplication>(
                                          (app) =>
                                          {
                                              Messenger.Send(app);
                                              AppNavigationService.GoToApplication();
                                              return TaskHelpers.FinishTask();
                                          }, AppResources.App_Loading));

            }
        }


        protected override void Save(IStorage storage)
        {
        }

        protected override void Load(IStorage storage)
        {
        }
    }
}
