using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class BaseFragment<T> : Fragment, INotifyPropertyChanged where T : AppViewModelBase
    {
        private readonly int _layout;
        public T ViewModel { get; private set; }
        protected MainActivity MainActivity { get { return (MainActivity)Activity; } }
        private ProgressDialog _dialog;

        public BaseFragment(int layout)
        {
            _layout = layout;
#if DEBUG

            try
            {
                ViewModel = (T)ViewModelManager.GetViewModel(typeof(T));

            }
            catch (Exception)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw;
            }

#else

            ViewModel = (T)ViewModelManager.GetViewModel(typeof(T));
#endif


        }

        protected T GetViewModel<T>()
        {
            return (T) ViewModelManager.GetViewModel(typeof (T));
        }

        public override void OnStart()
        {
            base.OnStart();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Context contextThemeWrapper = new ContextThemeWrapper(Activity, Resource.Style.None);

            // clone the inflater using the ContextThemeWrapper
            LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

            // inflate the layout using the cloned inflater, not default inflater
            var view = localInflater.Inflate(_layout, container, false);

            OnPrepareBindings(view);

            return view;
        }

        protected virtual void OnPrepareBindings(View view) { }


        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AppViewModelBase.GenericMessagePropertyName)
            {
                if (ViewModel.IsLoadingOperationRunning || _dialog != null)
                {
                    StartStopLoadingOperation();
                }
                else
                {
                    StartErrorToast();
                }
            }


        }

        private void StartErrorToast()
        {
            if (string.IsNullOrEmpty(ViewModel.GenericMessage))
                return;

            Toast.MakeText(Activity, ViewModel.GenericMessage, ToastLength.Short).Show();
        }

        private void StartStopLoadingOperation()
        {
            if (string.IsNullOrEmpty(ViewModel.GenericMessage))
            {
                // Operation ended
                using (_dialog)
                {
                    _dialog.Dismiss();
                }
                _dialog = null;
            }
            else
            {
                // New Operation
                _dialog = new ProgressDialog(Activity);
                _dialog.SetMessage(ViewModel.GenericMessage);
                _dialog.SetCanceledOnTouchOutside(false);
                _dialog.Show();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}