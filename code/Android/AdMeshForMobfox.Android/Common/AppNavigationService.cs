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
    public class AppNavigationService : IAppNavigationService
    {
        private readonly Activity _mainActivity;
        private Fragment _reportFragment;
        private Fragment _appFragment;

        public AppNavigationService(Activity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public void GoToLogin()
        {
            _mainActivity.NavigateToFragment(new LoginFragment(), false);
            _mainActivity.ActionBar.Hide();

        }

        public void GoToHome()
        {
            _mainActivity.NavigateToFragment(new HomeFragment(), false);
            _mainActivity.ActionBar.Show();
        }

        public void GoToReportDetails()
        {
            _mainActivity.NavigateToFragment(_reportFragment ?? (_reportFragment = new ReportFragment()));
        }

        public void GoToApplication()
        {
            _mainActivity.NavigateToFragment(_appFragment ?? (_appFragment = new ApplicationFragment()));

        }

        public void GoToCustomReport()
        {
            new CreateReportFragment().Show(_mainActivity.FragmentManager, typeof(CreateReportFragment).Name);
        }

        public void GoToAbout()
        {
            _mainActivity.NavigateToFragment(new AboutFragment());
        }


        private ICommand _goToLoginCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public ICommand GoToLoginCommand
        {
            get
            {
                return _goToLoginCommand
                    ?? (_goToLoginCommand = new RelayCommand(GoToLogin));
            }
        }

        private ICommand _goToHomeCommand;

        /// <summary>
        /// Gets the GoToHomeCommand.
        /// </summary>
        public ICommand GoToHomeCommand
        {
            get
            {
                return _goToHomeCommand
                    ?? (_goToHomeCommand = new RelayCommand(GoToHome));
            }
        }

        private ICommand _goToReportDetailsCommand;

        /// <summary>
        /// Gets the GoToReportDetailsCommand.
        /// </summary>
        public ICommand GoToReportDetailsCommand
        {
            get
            {
                return _goToReportDetailsCommand
                    ?? (_goToReportDetailsCommand = new RelayCommand(GoToReportDetails));
            }
        }

        private ICommand _goToAppCommand;

        /// <summary>
        /// Gets the GoToApplicationCommand.
        /// </summary>
        public ICommand GoToApplicationCommand
        {
            get
            {
                return _goToAppCommand
                    ?? (_goToAppCommand = new RelayCommand(GoToApplication));
            }
        }

        private ICommand _navToCreateRepoCommand;

        /// <summary>
        /// Gets the GoToCustomReportCommand.
        /// </summary>
        public ICommand GoToCustomReportCommand
        {
            get
            {
                return _navToCreateRepoCommand
                    ?? (_navToCreateRepoCommand = new RelayCommand(GoToCustomReport));
            }
        }

        private ICommand _navigateToAboutCommand;

        /// <summary>
        /// Gets the GoToAboutCommand.
        /// </summary>
        public ICommand GoToAboutCommand
        {
            get
            {
                return _navigateToAboutCommand
                    ?? (_navigateToAboutCommand = new RelayCommand(GoToAbout));
            }
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