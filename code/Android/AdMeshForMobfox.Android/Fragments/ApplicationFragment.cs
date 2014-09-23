using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.Localisation;
using AdMesh.Service.MobFox.Data;
using AdMesh.ViewModel;
using AdMeshForMobfox.Android.DumbBinding;
using AdMeshForMobfox.Android.Fragments.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments
{
    public class ApplicationFragment : BaseFragment<ApplicationViewModel>
    {
        public ApplicationFragment()
            : base(Resource.Layout.ApplicationLayout)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            MainActivity.SetActionBarForBackOperation(AppResources.App_Reports);
        }

        protected override void OnPrepareBindings(View view)
        {
            base.OnPrepareBindings(view);
            var icon = view.FindViewById<ImageView>(Resource.Id.app_layout_os);
            var name = view.FindViewById<TextView>(Resource.Id.app_app_name);

            var appBinding = ViewModel.CreateBinding(vm => vm.CurrentApplication);

            appBinding.BindTo(icon, app => app.GetPlatformImageId());
            appBinding.BindTo(name, app => app.Name);


            var overallReport = view.FindViewById<Button>(Resource.Id.app_overall_report);
            var yesterdayReport = view.FindViewById<Button>(Resource.Id.app_yesterday_report);
            var todayReport = view.FindViewById<Button>(Resource.Id.app_today_report);
            var thisWeekReport = view.FindViewById<Button>(Resource.Id.app_this_week_report);
            var lastWeekReport = view.FindViewById<Button>(Resource.Id.app_last_week_report);
            var thisMonthReport = view.FindViewById<Button>(Resource.Id.app_this_month_report);
            var lastMonthReport = view.FindViewById<Button>(Resource.Id.app_last_month_report);


            todayReport.Click += (s, a) => ViewModel.NavigateToTodayReportCommand.Execute(ViewModel.CurrentApplication);
            overallReport.Click += (s, a) => ViewModel.NavigateToOverallReportCommand.Execute(ViewModel.CurrentApplication);
            yesterdayReport.Click += (s, a) => ViewModel.NavigateToYesterdayCommand.Execute(ViewModel.CurrentApplication);
            thisWeekReport.Click += (s, a) => ViewModel.NavigateToThisWeekReportCommand.Execute(ViewModel.CurrentApplication);
            lastWeekReport.Click += (s, a) => ViewModel.NavigateToLastWeekReportCommand.Execute(ViewModel.CurrentApplication);
            thisMonthReport.Click += (s, a) => ViewModel.NavigateToThisMonthCommand.Execute(ViewModel.CurrentApplication);
            lastMonthReport.Click += (s, a) => ViewModel.NavigateToLastMonthCommand.Execute(ViewModel.CurrentApplication);
        }
    }
}