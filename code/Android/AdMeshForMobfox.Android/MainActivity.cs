using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;
using AdMesh.Common;
using AdMesh.Configurator;
using AdMesh.Configurator.Android;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using AdMeshForMobfox.Android.Common;
using AdMeshForMobfox.Android.Fragments;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Android.Ui;
using IOToolkit.Helpers;
using IOToolkit.Storage;

namespace AdMeshForMobfox.Android
{
    [Activity(
        Label = "AdMesh for Mobfox",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/BootTheme",
        MainLauncher = true,
        Icon = "@drawable/ic_launcher")]
    public class MainActivity : AppActivity
    {
        private DrawerLayout _drawer;
        private IMenu _menu;
        private ActionBarDrawerToggle _dashboardDrawerToggle;
        private List<string> _actionBartitles = new List<string>();


        public MainActivity()
            : base(Resource.Layout.Main)
        {
            Manager = new AndroidAppManager(this);
        }

        protected override Task Initialize()
        {
            _drawer = FindViewById<DrawerLayout>(Resource.Id.app_drawer);
            _drawer.DrawerClosed += (s, a) => InvalidateOptionsMenu();
            _drawer.DrawerOpened += (s, a) => InvalidateOptionsMenu();

            this.SetFragmentOnContainer(new SidebarFragment(), Resource.Id.SidebarArea, false,false);
            return TaskHelpers.FinishTask();
        }

        protected override Task PostInitialize()
        {
            return TaskHelpers.FinishTask();
        }

        protected override async Task ShowSplashScreen()
        {
            this.NavigateToFragment(new SplashscreenFragment(), false, false);
            await Task.Yield();
        }

        public void OpenCloseDrawer()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
                _drawer.CloseDrawer((int)GravityFlags.Left);
            else
                _drawer.OpenDrawer((int)GravityFlags.Left);

        }

        public void SetActionBarForDashboard()
        {
            HandleActionBarTitles();

            OptionsItemSelected -= OnBackPressed;

            SetRefreshVisibility(true);

            SetToggle(_dashboardDrawerToggle ?? (_dashboardDrawerToggle = new ActionBarDrawerToggle(
                this, /* host Activity */
                _drawer, /* DrawerLayout object */
                Resource.Drawable.ic_navigation_drawer,
                Resource.String.App_Open,
                Resource.String.App_Close
                )));
            _drawer.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
            _dashboardDrawerToggle.DrawerIndicatorEnabled = true;
        }



        public void SetActionBarForBackOperation(string title = null)
        {
            HandleActionBarTitles(title);
            SetRefreshVisibility(false);
            _dashboardDrawerToggle.DrawerIndicatorEnabled = false;
            OptionsItemSelected -= OnBackPressed;
            OptionsItemSelected += OnBackPressed;
            _drawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void HandleActionBarTitles(string newTitle = null)
        {
            if (string.IsNullOrEmpty(newTitle) && _actionBartitles.Count ==  0)
                return;

            if (_actionBartitles.Count != 0)
            {
                //
                //  Replace previous title
                //
                ActionBar.Title = _actionBartitles.Last();

                _actionBartitles.RemoveAt(_actionBartitles.Count - 1);

                return;
            }

            _actionBartitles.Add(ActionBar.Title);

            ActionBar.Title = newTitle;
        }

        void OnBackPressed(object sender, ActivityEventArgs e)
        {
            if ((sender as IMenuItem).ItemId == global::Android.Resource.Id.Home)
                FragmentManager.PopBackStack();
        }



        private void SetToggle(ActionBarDrawerToggle toggle)
        {
            _drawer.SetDrawerListener(toggle);

            toggle.SyncState();

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the menu items for use in the action bar
            MenuInflater.Inflate(Resource.Menu.app_actions, menu);
            _menu = menu;
            return base.OnCreateOptionsMenu(menu);
        }

        public MainActivity SetRefreshVisibility(bool isVisible)
        {
            SetItemVisibility(Resource.Id.action_bar_refresh, isVisible);
            return this;
        }

        private void SetItemVisibility(int id, bool isVisible)
        {
            if (_menu == null)
                return;

            var item = _menu.FindItem(id);
            item.SetVisible(isVisible);
        }

    }
}



