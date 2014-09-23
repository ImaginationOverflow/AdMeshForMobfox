using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using AdMesh.Service.MobFox.Data;
using AdMesh.ViewModel;
using AdMeshForMobfox.Android.Fragments.Adapters;
using AdMeshForMobfox.Android.Fragments.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace AdMeshForMobfox.Android.Fragments
{
    public class SidebarFragment : BaseFragment<HomeViewModel>
    {
        private ListViewHeightSetter _heightSetter;

        public SidebarFragment()
            : base(Resource.Layout.SidebarLayout)
        {
        }

        protected override void OnPrepareBindings(View view)
        {
            var listView = view.FindViewById<ListView>(Resource.Id.sidebar_app_list);
            var aboutBtn = view.FindViewById<LinearLayout>(Resource.Id.sidebar_about_btn);
            var customReportBtn = view.FindViewById<LinearLayout>(Resource.Id.sidebar_custom_report_btn);
            var sidebarBtn = view.FindViewById<LinearLayout>(Resource.Id.sidebar_rate_btn);


            aboutBtn.Click += (s, a) => ViewModel.AppNavigationService.GoToAboutCommand.Execute(null);
            sidebarBtn.Click += (s, a) => ShowStore();

            var adapter = new MobfoxApplicationCollectionAdapter(Activity, Resource.Layout.sidebar_app_layout,
                ViewModel.ApplicationsController.Applications);

            _heightSetter = new ListViewHeightSetter(listView);

            listView.Adapter = adapter;
            listView.ItemClick += listView_ItemClick;

            customReportBtn.Click += (s, a) => ViewModel.AppNavigationService.GoToCustomReportCommand.Execute(null);
        }

        private void ShowStore()
        {
            var uri = global::Android.Net.Uri.Parse("market://details?id=" + MainActivity.PackageName);
            
            
            Intent goToMarket = new Intent(Intent.ActionView, uri);
            try
            {
                MainActivity.StartActivity(goToMarket);
            }
            catch (ActivityNotFoundException e)
            {
                MainActivity.StartActivity(new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + MainActivity.PackageName)));
            }
        }


        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var list = sender as ListView;
            var adapter = list.Adapter as MobfoxApplicationCollectionAdapter;

            var app = adapter.GetItem(e.Position);

            ViewModel.NavigateToApplicationCommand.Execute(app);
        }
    }
}