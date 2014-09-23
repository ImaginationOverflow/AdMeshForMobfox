using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.Localisation;
using AdMesh.ViewModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments
{
    class AboutFragment : BaseFragment<HomeViewModel>
    {
        public AboutFragment() : base(Resource.Layout.AboutLayout)
        {
        }

        public override void OnStart()
        {
            MainActivity.SetActionBarForBackOperation(AppResources.App_About);
            base.OnStart();
        }
    }
}