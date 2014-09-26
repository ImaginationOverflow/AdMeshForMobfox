using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.Service.MobFox.Data;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments.Helpers
{
    static class FragmentHelpers
    {
        public static int GetPlatformImageId(this MobfoxApplication element)
        {
            switch (element.Type)
            {
                case MobfoxApplication.WindowsPhoneApp:
                    return Resource.Drawable.ic_wp;
                case MobfoxApplication.Androidpp:
                    return Resource.Drawable.ic_android;
                default:
                    return Resource.Drawable.ic_ios;
            }
        }

        public static string ToDateString(this DateTime value)
        {
            return value.ToString("d");
        }

    }
}