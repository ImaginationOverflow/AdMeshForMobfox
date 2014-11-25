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
using IOToolkit.ObjectModel;
using IOToolkit.Storage;

namespace AdMesh.ViewModel
{
    public class ReportViewModel : AppViewModelBase
    {
        public ApplicationsController ApplicationsController { get; private set; }
        private readonly ReportsController _controller;
        private readonly IAppNavigationService _navigationService;

        /// <summary>
        /// The <see cref="Report" /> property's name.
        /// </summary>
        public const string ReportPropertyName = "Report";

        private MobfoxReport _report;

        /// <summary>
        /// Sets and gets the Report property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport Report
        {
            get
            {
                return _report;
            }

            set
            {
                if (_report == value)
                {
                    return;
                }

                _report = value;
                RaisePropertyChanged(ReportPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StartDate" /> property's name.
        /// </summary>
        public const string StartDatePropertyName = "StartDate";

        private DateTime _startDateTime = DateTime.Now;

        /// <summary>
        /// Sets and gets the StartDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return _startDateTime;
            }

            set
            {
                if (_startDateTime == value)
                {
                    return;
                }

                _startDateTime = value;
                RaisePropertyChanged(StartDatePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="EndDate" /> property's name.
        /// </summary>
        public const string EndDatePropertyName = "EndDate";

        private DateTime _endDateTime = DateTime.Now;

        /// <summary>
        /// Sets and gets the EndDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _endDateTime;
            }

            set
            {
                if (_endDateTime == value)
                {
                    return;
                }

                _endDateTime = value;
                RaisePropertyChanged(EndDatePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Application" /> property's name.
        /// </summary>
        public const string ApplicationPropertyName = "Application";

        private MobfoxApplication _application = null;

        /// <summary>
        /// Sets and gets the Application property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxApplication Application
        {
            get
            {
                return _application;
            }

            set
            {
                if (_application == value)
                {
                    return;
                }

                _application = value;
                RaisePropertyChanged(ApplicationPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsOverall" /> property's name.
        /// </summary>
        public const string IsOverallPropertyName = "IsOverall";

        private bool _isOverall = false;

        /// <summary>
        /// Sets and gets the IsOverall property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsOverall
        {
            get
            {
                return _isOverall;
            }

            set
            {
                if (_isOverall == value)
                {
                    return;
                }

                _isOverall = value;
                RaisePropertyChanged(IsOverallPropertyName);
            }
        }

        public ReportViewModel(ReportsController controller, ApplicationsController applicationsController, IAppNavigationService navigationService)
        {
            ApplicationsController = applicationsController;
            _controller = controller;
            _navigationService = navigationService; 
            Messenger.Register<ReportMessage>(this, OnNewReport);
        }

        private ICommand _clearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand
                    ?? (_clearCommand = CreateSafeCommand(
                                          () =>
                                          {
                                              Clear();
                                              return TaskHelpers.FinishTask();
                                          }));
            }
        }

        private IAsyncCommand _loadReportCommand;

        /// <summary>
        /// Gets the LoadReportCommand.
        /// </summary>
        public IAsyncCommand LoadReportCommand
        {
            get
            {
                return _loadReportCommand 
                    ?? (_loadReportCommand = CreateAsyncCommandForUserAction(
                                          async () =>
                                          {
                                              if (IsOverall)
                                                  Report = await _controller.GetReport(null, null, Application);
                                              else
                                                  Report = await _controller.GetReport(StartDate, EndDate, Application);

                                              _navigationService.GoToReportDetails();
                                          },AppResources.App_Loading));
										    
            }
        }

        private void OnNewReport(ReportMessage obj)
        {
            Report = obj.Report;
        }


        private void Clear()
        {
            Application = null;
            StartDate = EndDate = DateTime.Now;
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
