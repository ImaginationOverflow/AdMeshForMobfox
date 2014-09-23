using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AdMesh.Localisation;
using AdMesh.Service.MobFox.Data;
using AdMeshForMobfox.Android.Fragments.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments.Adapters
{
    class MobfoxAppSelectionAdapter : ObservableCollectionAdapter<MobfoxApplication>
    {
        public MobfoxAppSelectionAdapter(Context context, int layoutId, ObservableCollection<MobfoxApplication> collection)
            : base(context, layoutId, collection)
        {
            Add(new MobfoxApplication { Name = AppResources.App_None });
        }

    }
}