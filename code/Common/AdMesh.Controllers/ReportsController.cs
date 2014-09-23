using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Controllers.Data;
using AdMesh.Controllers.Extensions;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using AdMesh.Storage;
using GalaSoft.MvvmLight;
using IOToolkit.Controllers;
using IOToolkit.Helpers;
using IOToolkit.Helpers.Time;
using IOToolkit.Helpers.Time.DateTime;

namespace AdMesh.Controllers
{
    public class ReportsController : ObservableObject, IDataController
    {
        private readonly IMobfoxService _mobfoxService;
        private readonly DashboardStorage _storage;

        /// <summary>
        /// The <see cref="DashboardInfo" /> property's name.
        /// </summary>
        public const string DashboardInfoPropertyName = "DashboardInfo";

        private DashboardInfo _dashboardInfo;

        /// <summary>
        /// Sets and gets the DashboardInfo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DashboardInfo DashboardInfo
        {
            get
            {
                return _dashboardInfo;
            }

            set
            {
                if (_dashboardInfo == value)
                {
                    return;
                }

                _dashboardInfo = value;
                RaisePropertyChanged(DashboardInfoPropertyName);
            }
        }

        public ReportsController(IMobfoxService mobfoxService, DashboardStorage storage)
        {
            _mobfoxService = mobfoxService;
            _storage = storage;
        }


        public Task<MobfoxReport> GetTodayReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetTodayReport(app);
        }

        public Task<MobfoxReport> GetThisWeekReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetThisWeekReport(app);
        }

        public Task<MobfoxReport> GetThisMonthReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetThisMonthReport(app);
        }

        public Task<MobfoxReport> GetLastWeekReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetLastWeekReport(app);
        }

        public Task<MobfoxReport> GetLastMonthReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetLastMonthReport(app);
        }

        public Task<MobfoxReport> GetOverallReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetReportAsync(app);
        }

        public Task<MobfoxReport> GetYesterdayReport(MobfoxApplication app = null)
        {
            return _mobfoxService.GetYesterdayReport(app);
        }

        public Task<MobfoxReport> GetDailyReport(DateTime date, MobfoxApplication app = null)
        {
            return _mobfoxService.GetReportOfDate(date, app);
        }

        public Task<MobfoxReport> GetReport(DateTime? startDate, DateTime? endDate, MobfoxApplication app = null)
        {
            return _mobfoxService.GetReportAsync(app, startDate, endDate);
        }


        public async Task RefreshDashboardAsync()
        {

            var currentDate = DateTime.Now.Date;
            if (DashboardInfo == null)
            {
                await LoadDashboardAsync();
                return;
            }

            DashboardInfo.TodaysReport = await GetTodayReport();
            DashboardInfo.ThisWeekReport = await GetThisWeekReport();
            DashboardInfo.ThisMonthReport = await GetThisMonthReport();



            if (DashboardInfo.LastWeekReport.StartTime.Value.Date != currentDate.FirstDayOfWeek().WeekEarlier())
            {
                DashboardInfo.LastWeekReport = await GetLastWeekReport();
            }

            if (DashboardInfo.LastMonthReport.StartTime.Value.Date != currentDate.FirstDayOfMonth().PreviousMonth())
            {
                DashboardInfo.LastMonthReport = await GetLastMonthReport();
            }


            var lastDayOnDailyReport = DashboardInfo.LastWeekDailyReports.Max(e => e.StartTime).Value.Date;

            if (lastDayOnDailyReport == currentDate.PreviousDay())
                return;

            if (lastDayOnDailyReport < currentDate.Subtract(TimeSpan.FromDays(7)))
            {
                await LoadDailyReports(DashboardInfo);
                return;
            }

            var reports = DashboardInfo.LastWeekDailyReports;
            for (var date = lastDayOnDailyReport.NextDay(); date < currentDate; date = date.NextDay())
            {
                var report = await GetDailyReport(date);

                if (reports.Count != 0)
                    reports.RemoveAt(0);

                reports.Add(report);


            }
        }

        private async Task LoadDashboardAsync()
        {
            var dashboard = new DashboardInfo();
            Func<Task> todayMethod = async () =>
            {
                dashboard.TodaysReport =
                    await GetTodayReport();
            };

            Func<Task> thisWeek = async () =>
            {
                dashboard.ThisWeekReport =
                    await GetThisWeekReport();
            };

            Func<Task> lastWeek = async () =>
            {
                dashboard.LastWeekReport =
                    await GetLastWeekReport();
            };

            Func<Task> thisMonth = async () =>
            {
                dashboard.ThisMonthReport =
                    await GetThisMonthReport();
            };

            Func<Task> lastMonth = async () =>
            {
                dashboard.LastMonthReport =
                    await GetLastMonthReport();
            };


            await Task.WhenAll(
            new[]{
                    todayMethod(),
                    thisWeek(),
                    lastWeek(),
                    thisMonth(),
                    lastMonth()

                    }
            );

            await LoadDailyReports(dashboard);

            DashboardInfo = dashboard;
        }

        private async Task LoadDailyReports(DashboardInfo dashboard)
        {
            var weekDailyReports =
                0.To(7)
                    .Select(async i => await GetDailyReport(DateTime.Now.Subtract(TimeSpan.FromDays(i + 1))))
                    .ToList();


            var reports = await Task.WhenAll(weekDailyReports);

            dashboard.LastWeekDailyReports = new ObservableCollection<MobfoxReport>(reports.OrderBy(e => e.StartTime));
        }

        public async Task Load()
        {
            DashboardInfo = await _storage.Load();
        }

        public Task Save()
        {
            return _storage.Save(DashboardInfo);
        }
    }
}
