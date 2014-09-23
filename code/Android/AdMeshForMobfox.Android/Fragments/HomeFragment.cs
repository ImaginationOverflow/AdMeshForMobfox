using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Common;
using AdMesh.Controllers;
using AdMesh.Controllers.Data;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using AdMesh.ViewModel;
using AdMeshForMobfox.Android.DumbBinding;
using AdMeshForMobfox.Android.DumbBinding;
using AdMeshForMobfox.Android.Fragments.Helpers;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Dalvik.SystemInterop;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.XamarinAndroid;
using BinderExtensions = AdMeshForMobfox.Android.DumbBinding.BinderExtensions;

namespace AdMeshForMobfox.Android.Fragments
{
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        /// <summary>
        /// The <see cref="ChartValues" /> property's name.
        /// </summary>
        public const string ChartValuesPropertyName = "ChartValues";

        private List<AreaChartData> _chartValues;
        private PlotView _plot;

        /// <summary>
        /// Sets and gets the ChartData property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<AreaChartData> ChartValues
        {
            get
            {
                return _chartValues;
            }

            set
            {
                if (_chartValues == value)
                {
                    return;
                }

                _chartValues = value;
                RaisePropertyChanged(ChartValuesPropertyName);
            }
        }



        public HomeFragment()
            : base(Resource.Layout.HomeLayout)
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel.RefreshCommand.Execute(null);
        }

        public override void OnStart()
        {
            base.OnStart();

            MainActivity.OptionsItemSelected += MainActivity_OptionsItemSelected;
            MainActivity.SetActionBarForDashboard();
        }

        public override void OnPause()
        {
            base.OnPause();
            MainActivity.OptionsItemSelected -= MainActivity_OptionsItemSelected;

        }


        void MainActivity_OptionsItemSelected(object sender, ActivityEventArgs e)
        {
            var item = sender as IMenuItem;

            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    MainActivity.OpenCloseDrawer();
                    break;
                case Resource.Id.action_bar_refresh:
                    ViewModel.RefreshCommand.Execute(null);
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }


        protected async override void OnPrepareBindings(View view)
        {

            _plot = view.FindViewById<PlotView>(Resource.Id.home_plotview);



            MainActivity.ActionBar.SetHomeButtonEnabled(true);
            MainActivity.ActionBar.SetDisplayHomeAsUpEnabled(true);
            var todayReportContainer = view.FindViewById<LinearLayout>(Resource.Id.home_today_report);
            var thisWeekContainer = view.FindViewById<LinearLayout>(Resource.Id.home_this_week_report);
            var lastWeekContainer = view.FindViewById<LinearLayout>(Resource.Id.home_last_week_report);
            var thisMonthContainer = view.FindViewById<LinearLayout>(Resource.Id.home_this_month_report);
            var lastMonthContainer = view.FindViewById<LinearLayout>(Resource.Id.home_last_month_report);

            todayReportContainer.Click +=
                (s, a) =>
                    ViewModel.ShowReportDetailsCommand.Execute(ViewModel.ReportsController.DashboardInfo.TodaysReport);

            thisWeekContainer.Click +=
               (s, a) =>
                   ViewModel.ShowReportDetailsCommand.Execute(ViewModel.ReportsController.DashboardInfo.ThisWeekReport);
            lastWeekContainer.Click +=
               (s, a) =>
                   ViewModel.ShowReportDetailsCommand.Execute(ViewModel.ReportsController.DashboardInfo.LastWeekReport);
            thisMonthContainer.Click +=
               (s, a) =>
                   ViewModel.ShowReportDetailsCommand.Execute(ViewModel.ReportsController.DashboardInfo.ThisMonthReport);
            lastMonthContainer.Click +=
               (s, a) =>
                   ViewModel.ShowReportDetailsCommand.Execute(ViewModel.ReportsController.DashboardInfo.LastMonthReport);




            var clicks = view.FindViewById<TextView>(Resource.Id.home_clicks);
            var ctr = view.FindViewById<TextView>(Resource.Id.home_ctr);
            var ecpm = view.FindViewById<TextView>(Resource.Id.home_ecpm);
            var impressions = view.FindViewById<TextView>(Resource.Id.home_impressions);
            var lastMonth = view.FindViewById<TextView>(Resource.Id.home_last_month_revenue);
            var thisMonth = view.FindViewById<TextView>(Resource.Id.home_this_month_revenue);
            var lastWeek = view.FindViewById<TextView>(Resource.Id.home_last_week_revenue);
            var thisWeek = view.FindViewById<TextView>(Resource.Id.home_this_week_revenue);
            var today = view.FindViewById<TextView>(Resource.Id.home_today_revenue);



            var dashboardBinding = ViewModel.ReportsController.CreateBindingChain(c => c.DashboardInfo);

            var todayBinding = dashboardBinding.CreateBinding(d => d.TodaysReport);

            todayBinding.BindTo(ecpm, report => report.Ecpm.ToPrettyString());

            todayBinding.BindTo(clicks, report => report.Clicks);

            todayBinding.BindTo(ctr, report => report.Ctr.ToPercentagePrettyString());


            todayBinding.BindTo(impressions, report => report.Impressions);

            todayBinding.BindTo(today, r => r.Earnings.ToPrettyString());

            dashboardBinding.CreateBinding(d => d.LastMonthReport)
                .BindTo(lastMonth, r => r.Earnings.ToPrettyString());

            dashboardBinding.CreateBinding(d => d.ThisMonthReport)
               .BindTo(thisMonth, r => r.Earnings.ToPrettyString());

            dashboardBinding.CreateBinding(d => d.LastWeekReport)
               .BindTo(lastWeek, r => r.Earnings.ToPrettyString());

            dashboardBinding.CreateBinding(d => d.ThisWeekReport)
             .BindTo(thisWeek, r => r.Earnings.ToPrettyString());



            PropertyChanged += SetChartData;

            dashboardBinding.CreateBinding(d => d.LastWeekDailyReports)
                .BindTo(this, f => f.ChartValues, d => new List<AreaChartData>(d.Select(i => new AreaChartData
                {
                    Time = i.EndTime.Value,
                    Value = i.Earnings.Amount
                })));



            HandleAdBanner(view);
        }

        private void HandleAdBanner(View view)
        {
            var ad = new AdView(MainActivity)
            {
                AdSize = AdSize.SmartBanner,
                AdUnitId = "ca-app-pub-4818186136260776/5371438043"
            };
            var requestbuilder = new AdRequest.Builder();
            ad.LoadAd(requestbuilder.Build());
            var layout = view.FindViewById<LinearLayout>(Resource.Id.home_ad_banner);
            layout.AddView(ad);
        }

        private void SetChartData(object sender, PropertyChangedEventArgs e)
        {
            var plotModel = new PlotModel();
            plotModel.PlotAreaBorderColor = plotModel.Background = OxyColors.Transparent;

            plotModel.Axes.Add(new LinearAxis
           {
               IsAxisVisible = true,
               AxislineThickness = 1,
               AxislineColor = OxyColors.Black,
               IsZoomEnabled = false,
               IsPanEnabled = false,
               Position = AxisPosition.Left,
               TickStyle = TickStyle.Crossing,
               AxislineStyle = LineStyle.Solid,
               Minimum = 0,
               MinimumPadding = 0,
               MaximumPadding = 0,
           });


            plotModel.Axes.Add(new DateTimeAxis
            {
                IsZoomEnabled = false,
                IsPanEnabled = false,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Outside,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "dd/MM",
                AxislineThickness = 1,
                AxislineStyle = LineStyle.Solid,
                MinimumPadding = 0,
                MaximumPadding = 0,
            });

            plotModel.Padding = new OxyThickness(18);
            plotModel.Series.Add(GetData());
            _plot.Model = plotModel;
        }

        public class AreaChartData
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
            public double Zero { get; set; }
        }

        private Series GetData()
        {
            var accentColor = Resources.GetColor(Resource.Color.app_accent_color);
            var areaSeries1 = new AreaSeries();
            {

            };
            areaSeries1.Color = OxyColor.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);

            areaSeries1.DataFieldX = "Time";
            areaSeries1.DataFieldY = "Value";


            areaSeries1.DataFieldX2 = "Time";
            areaSeries1.DataFieldY2 = "Zero";
            areaSeries1.Color2 = OxyColors.Transparent;


            areaSeries1.ItemsSource = ChartValues;
            return areaSeries1;
        }
    }
}