using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Configurator;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android
{

    public class ActivityEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }




    public abstract class AppActivity : Activity
    {
        /// <summary>
        /// Name of key that should be added on the bundle when the app is tombstoned
        /// </summary>
        private const string AppTombstoned = "_tomstoned";

        /// <summary>
        /// App Manager
        /// </summary>
        protected  AppManagerBase Manager { get; set; }

        /// <summary>
        /// The app base layout
        /// </summary>
        private readonly int _appLayout;

        /// <summary>
        /// Flag to mark if the app is closing or being tombstoned
        /// </summary>
        private bool _isTombstoning;

        #region Events

        /// <summary>
        /// Called when OnOptionsItemSelected is called
        /// </summary>
        public event EventHandler<ActivityEventArgs> OptionsItemSelected;
        public event EventHandler<EventArgs> OnBackKeyPressed;

       

        #endregion

        protected AppActivity(int appLayout)
        {
            _appLayout = appLayout;
        }





        protected override async void OnCreate(Bundle bundle)
        {
#if DEBUG
            try
            {
#endif
                //
                //  Register events to catch all unhandled exceptions
                //  On debug a dialog will show the error
                //  On production the Manager will handle it, by saving the exception detail and 
                //  ask the user if they want to send it
                //
                AndroidEnvironment.UnhandledExceptionRaiser +=
                    (sender, args) => HandleUnhandledException(args.Exception);

                AppDomain.CurrentDomain.UnhandledException +=
                    (s, e) => HandleUnhandledException(e.ExceptionObject as Exception);

                //
                //  Pre initialize the app, things like theme setting should be done here
                //
                Manager.PreInitialize();

                base.OnCreate(bundle);

                SetContentView(_appLayout);

                //
                //  Show splash before starting to boot the app
                //
                await ShowSplashScreen();

                Task initializationTask;

                if (bundle != null && bundle.ContainsKey(AppTombstoned))
                    initializationTask = Manager.Activated(false);
                else
                    initializationTask = Manager.Initialize();


                //
                //  Initialize all ui and non ui related stuff
                //
                await initializationTask;
                await Initialize();

                //
                //  Start navigation
                //
                Manager.Start();

                //
                //  For activating some ui controls or whatever is needed after the app initialization
                //
                await PostInitialize();
#if DEBUG
            }
            catch (Exception e)
            {
                ShowError(e);
            }
#endif
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            Task.Run(async () => await Manager.Deactivated()).Wait();

            outState.PutString(AppTombstoned, AppTombstoned);
            _isTombstoning = true;
        }

        protected override async void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            await Manager.Activated(true);
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (!_isTombstoning)
                Task.Run(async () => await Manager.Closing()).Wait();

            _isTombstoning = false;
        }


        protected abstract Task ShowSplashScreen();

        /// <summary>
        /// To configure app specific controls and operations
        /// </summary>
        /// <returns></returns>
        protected abstract Task Initialize();

        /// <summary>
        /// The app is ready to show to the user, add some after initialization code here
        /// </summary>
        /// <returns></returns>
        protected abstract Task PostInitialize();


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            EventHandler<ActivityEventArgs> handler = OptionsItemSelected;

            if (handler != null)
            {
                var args = new ActivityEventArgs();

                foreach (EventHandler<ActivityEventArgs> @delegate in handler.GetInvocationList())
                {
                    @delegate(item, args);

                    if (args.Handled)
                        break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            EventHandler<EventArgs> handler = OnBackKeyPressed;
            if (handler != null) handler(this, EventArgs.Empty);

            base.OnBackPressed();
        }

      
#if DEBUG

        private void ShowError(object o)
        {
            new AlertDialog.Builder(this).SetMessage(o.ToString()).Show();
        }

        private void HandleUnhandledException(Exception e)
        {
            ShowError(e);
        }
#else
        private void HandleUnhandledException(Exception e)
        {
            Manager.OnUnhandledException(e);
        }
#endif




    }
}