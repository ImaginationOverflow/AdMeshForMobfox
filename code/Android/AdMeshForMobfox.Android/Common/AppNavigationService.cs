using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AdMesh.Common;
using AdMeshForMobfox.Android.Fragments;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOToolkit.ObjectModel;

namespace AdMeshForMobfox.Android.Common
{
    public class AppNavigationService : AppNavigationServiceBase
    {
        private readonly Activity _mainActivity;
        private Fragment _reportFragment;
        private Fragment _appFragment;

        public AppNavigationService(Activity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public override void GoToLogin()
        {
            _mainActivity.NavigateToFragment(new LoginFragment(), false);
            _mainActivity.ActionBar.Hide();

        }

        public override void GoToHome()
        {
            _mainActivity.NavigateToFragment(new HomeFragment(), false);
            _mainActivity.ActionBar.Show();
        }

        public override void GoToReportDetails()
        {
            _mainActivity.NavigateToFragment(_reportFragment ?? (_reportFragment = new ReportFragment()));
        }

        public override void GoToApplication()
        {
            _mainActivity.NavigateToFragment(_appFragment ?? (_appFragment = new ApplicationFragment()));

        }

        public override void GoToCustomReport()
        {
            new CreateReportFragment().Show(_mainActivity.FragmentManager, typeof(CreateReportFragment).Name);
        }

        public override void GoToAbout()
        {
            _mainActivity.NavigateToFragment(new AboutFragment());
        }



    }

    public static class FragmentExtensions
    {
        internal static void NavigateToFragment(this Activity mainActivity, Fragment fragment, bool addToBackStack = true, bool animate = true)
        {
            mainActivity.SetFragmentOnContainer(fragment, Resource.Id.ContentArea, addToBackStack, animate);
        }

        internal static void SetFragmentOnContainer(this Activity mainActivity, Fragment fragment, int fragmentContainer, bool addToBackStack = true, bool animate = true)
        {
            try
            {
                var fragName = fragment.GetType().Name;

                var trans = mainActivity.FragmentManager.BeginTransaction();
                if (animate)
                    trans.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter,
                        Resource.Animation.popexit);

                trans.Replace(fragmentContainer, fragment, fragName);

                if (addToBackStack)
                {
                    trans.AddToBackStack(fragName);
                }

                trans.Commit();
            }
            catch (Exception)
            {

            }

        }
    }
}