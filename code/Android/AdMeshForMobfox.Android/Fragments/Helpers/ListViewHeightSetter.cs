using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments.Helpers
{
    /// <summary>
    /// Sets the height on list views, due to android awesomeness, scrollview and listview don't go well together
    /// This automatically sets the list height making it possible to use it inside a scrollviewer while receiving click events
    /// </summary>
    public class ListViewHeightSetter
    {
        private readonly ListView _list;
        private readonly INotifyCollectionChanged _data;

        public ListViewHeightSetter(ListView list)
        {
            _list = list;
            _list.LayoutChange += _list_LayoutChange;
        }

        void _list_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            SetListViewSize(_list);
        }


        /// <summary>
        /// Sets the list view with a specific size, by doing so it can go inside scrollviews with no problems
        /// and since the overall size is set the listview scroll wont be enable.
        /// </summary>
        /// <param name="myListView"></param>
        public static void SetListViewSize(ListView myListView)
        {
            IListAdapter myListAdapter = myListView.Adapter;
            if (myListAdapter == null)
            {
                //do nothing return null
                return;
            }
            //set listAdapter in loop for getting final size
            int totalHeight = 0;
            for (int size = 0; size < myListAdapter.Count; size++)
            {
                var listItem = myListAdapter.GetView(size, null, myListView);
                listItem.Measure(0, 0);
                totalHeight += listItem.MeasuredHeight;
            }
            //setting listview item in adapter
            ViewGroup.LayoutParams @params = myListView.LayoutParameters;
            @params.Height = totalHeight + (myListView.DividerHeight * (myListAdapter.Count - 1));
            myListView.LayoutParameters = @params;
        }
    }
}