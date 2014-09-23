using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AdMesh.Service.MobFox.Data;
using GalaSoft.MvvmLight;

namespace AdMesh.Controllers.Data
{
    public class DashboardInfo : ObservableObject
    {
        /// <summary>
        /// The <see cref="TodaysReport" /> property's name.
        /// </summary>
        public const string TodaysReportPropertyName = "TodaysReport";

        private MobfoxReport _todayReport = null;

        /// <summary>
        /// Sets and gets the TodaysReport property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport TodaysReport
        {
            get
            {
                return _todayReport;
            }

            set
            {
                if (_todayReport == value)
                {
                    return;
                }

                _todayReport = value;
                RaisePropertyChanged(TodaysReportPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LastWeekReport" /> property's name.
        /// </summary>
        public const string LastWeekReportPropertyName = "LastWeekReport";

        private MobfoxReport _lastWeekReport;

        /// <summary>
        /// Sets and gets the LastWeekReport property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport LastWeekReport
        {
            get
            {
                return _lastWeekReport;
            }

            set
            {
                if (_lastWeekReport == value)
                {
                    return;
                }

                _lastWeekReport = value;
                RaisePropertyChanged(LastWeekReportPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ThisWeekReport" /> property's name.
        /// </summary>
        public const string ThisWeekReportPropertyName = "ThisWeekReport";

        private MobfoxReport _thisweekReport ;

        /// <summary>
        /// Sets and gets the ThisWeekReport property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport ThisWeekReport
        {
            get
            {
                return _thisweekReport;
            }

            set
            {
                if (_thisweekReport == value)
                {
                    return;
                }

                _thisweekReport = value;
                RaisePropertyChanged(ThisWeekReportPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LastMonthReport" /> property's name.
        /// </summary>
        public const string LastMonthReportPropertyName = "LastMonthReport";

        private MobfoxReport _lastMonthReport ;

        /// <summary>
        /// Sets and gets the LastMonthReport property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport LastMonthReport
        {
            get
            {
                return _lastMonthReport;
            }

            set
            {
                if (_lastMonthReport == value)
                {
                    return;
                }

                _lastMonthReport = value;
                RaisePropertyChanged(LastMonthReportPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ThisMonthReport" /> property's name.
        /// </summary>
        public const string ThisMonthReportPropertyName = "ThisMonthReport";

        private MobfoxReport _thisMonthReport;

        /// <summary>
        /// Sets and gets the ThisMonthReport property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxReport ThisMonthReport
        {
            get
            {
                return _thisMonthReport;
            }

            set
            {
                if (_thisMonthReport == value)
                {
                    return;
                }

                _thisMonthReport = value;
                RaisePropertyChanged(ThisMonthReportPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="LastWeekDailyReports" /> property's name.
        /// </summary>
        public const string LastWeekDailyReportsPropertyName = "LastWeekDailyReports";

        private ObservableCollection<MobfoxReport> _reports = new ObservableCollection<MobfoxReport>();

        /// <summary>
        /// Sets and gets the LastWeekDailyReports property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MobfoxReport> LastWeekDailyReports
        {
            get
            {
                return _reports;
            }

            set
            {
                if (_reports == value)
                {
                    return;
                }

                _reports = value;
                RaisePropertyChanged(LastWeekDailyReportsPropertyName);
            }
        }

    }
}
