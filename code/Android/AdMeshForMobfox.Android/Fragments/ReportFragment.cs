using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class ReportFragment : BaseFragment<ReportViewModel>
    {
        public ReportFragment() : base(Resource.Layout.ReportLayout)
        {
        }

        public override void OnStart()
        {
            base.OnStart();
            MainActivity.SetActionBarForBackOperation();
        }

        protected override void OnPrepareBindings(View view)
        {
            base.OnPrepareBindings(view);

            var impressions = view.FindViewById<TextView>(Resource.Id.report_impressions);
            var clicks = view.FindViewById<TextView>(Resource.Id.report_clicks);
            var ctr = view.FindViewById<TextView>(Resource.Id.report_ctr);
            var cpc = view.FindViewById<TextView>(Resource.Id.report_cpc);
            var revenue = view.FindViewById<TextView>(Resource.Id.report_revenue);
            var ecpm = view.FindViewById<TextView>(Resource.Id.report_ecpm);

            var reportBinding = ViewModel.CreateBinding(e => e.Report);

            reportBinding.BindTo(impressions, e => e.Impressions);
            reportBinding.BindTo(clicks, e => e.Clicks);
            reportBinding.BindTo(ctr, e => e.Ctr.ToPercentagePrettyString());
            reportBinding.BindTo(cpc, e => e.AverageCpc.ToPrettyString());
            reportBinding.BindTo(revenue, e => e.Earnings.ToPrettyString());
            reportBinding.BindTo(ecpm, e => e.Ecpm.ToPrettyString());
        }
    }
}