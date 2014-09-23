using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments.Adapters
{
    public class ObservableCollectionAdapter<T> : ArrayAdapter<T>
    {


        protected readonly int LayoutId;
        protected readonly ObservableCollection<T> Collection;
        protected readonly LayoutInflater Inflater;

        public ObservableCollectionAdapter(Context context, int layoutId, ObservableCollection<T> collection)
            : base(context, layoutId, collection)
        {
            LayoutId = layoutId;
            Collection = collection;
            Inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            collection.CollectionChanged += collection_CollectionChanged;
        }



        void collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                return;

            }

            if (e.OldItems != null)
                foreach (T oldItem in e.OldItems)
                {
                    Remove(oldItem);
                }

            if (e.NewItems != null)
                AddAll(e.NewItems);

            

        }
    }
}