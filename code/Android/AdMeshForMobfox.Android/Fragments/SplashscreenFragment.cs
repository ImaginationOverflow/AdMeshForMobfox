using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.ViewModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments
{
    public class SplashscreenFragment : Fragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Context contextThemeWrapper = new ContextThemeWrapper(Activity, Resource.Style.None);

            // clone the inflater using the ContextThemeWrapper
            LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

            // inflate the layout using the cloned inflater, not default inflater
            var view = localInflater.Inflate(Resource.Layout.SplashscreenLayout, container, false);


            return view;
        }

    }
}