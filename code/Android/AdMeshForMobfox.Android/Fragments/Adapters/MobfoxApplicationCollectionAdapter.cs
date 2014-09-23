using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AdMesh.Service.MobFox.Data;
using AdMeshForMobfox.Android.Fragments.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Text;

namespace AdMeshForMobfox.Android.Fragments.Adapters
{
    public class MobfoxApplicationCollectionAdapter : ObservableCollectionAdapter<MobfoxApplication>
    {

        public MobfoxApplicationCollectionAdapter(Context context, int layoutId, ObservableCollection<MobfoxApplication> collection)
            : base(context, layoutId, collection)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var view = convertView ?? Inflater.Inflate(LayoutId, parent, false);


            var text = view.FindViewById<TextView>(Resource.Id.sidebar_app_layout_name);
            var image = view.FindViewById<ImageView>(Resource.Id.sidebar_app_layout_os);

            var element = Collection[position];

            image.SetImageDrawable(base.Context.Resources.GetDrawable(element.GetPlatformImageId()));
            text.Text = element.Name;

            return view;
        }

       
    }
}