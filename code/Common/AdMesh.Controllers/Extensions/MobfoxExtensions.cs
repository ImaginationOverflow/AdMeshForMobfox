using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using IOToolkit.Helpers.Time.DateTime;

namespace AdMesh.Controllers.Extensions
{
    static class MobfoxExtensions
    {

        public static async Task<MobfoxReport> GetReportOfDate(this IMobfoxService mobfox, DateTime date, MobfoxApplication app)
        {
            return await mobfox.GetReportAsync(app, startDate: date, endDate: date);

        }

        public static async Task<MobfoxReport> GetTodayReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            return await mobfox.GetReportOfDate(DateTime.Now, app);
        }

        public static async Task<MobfoxReport> GetYesterdayReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            return await mobfox.GetReportOfDate(DateTime.Now.PreviousDay(), app);
        }

        public static async Task<MobfoxReport> GetThisWeekReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            var currentTime = DateTime.Now;
            return await mobfox.GetReportAsync(app, startDate: currentTime.FirstDayOfWeek(), endDate: currentTime.LastDayOfWeek());
        }

        public static async Task<MobfoxReport> GetLastWeekReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            var currentTime = DateTime.Now;
            return await mobfox.GetReportAsync(app, currentTime.FirstDayOfWeek().WeekEarlier(), currentTime.LastDayOfWeek().WeekEarlier());
        }

        public static async Task<MobfoxReport> GetThisMonthReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            var currentTime = DateTime.Now;
            return await mobfox.GetReportAsync(app, currentTime.FirstDayOfMonth(), currentTime);
        }

        public static async Task<MobfoxReport> GetLastMonthReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            var currentTime = DateTime.Now;
            var previusMonth = currentTime.PreviousMonth();
            return await mobfox.GetReportAsync(app, previusMonth.FirstDayOfMonth(), previusMonth.LastDayOfMonth());
        }

        public static async Task<MobfoxReport> GetOverallReport(this IMobfoxService mobfox, MobfoxApplication app = null)
        {
            return await mobfox.GetReportAsync(app);
        }

    }
}
