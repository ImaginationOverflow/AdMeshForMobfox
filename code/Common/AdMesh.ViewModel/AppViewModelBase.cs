using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdMesh.Localisation;
using AdMesh.Localisation;
using GalaSoft.MvvmLight;
using IOToolkit.Messaging;
using IOToolkit.ObjectModel;
using IOToolkit.Services;
using IOToolkit.Storage;

namespace AdMesh.ViewModel
{
    public abstract class AppViewModelBase : ViewModelBase
    {

        public const int ShowErrorTime = 3000;

        internal IMessenger Messenger { get { return AppCommonServicesContainer.Instance.Messenger; } }
        internal IMessageBoxService MessageBoxService { get { return AppCommonServicesContainer.Instance.MessageBoxService; } }


        public AppViewModelBase()
        {

        }


        internal async Task SafeRun(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        internal async Task LoadingSafeRun(Func<Task> func, string loadingMessage, bool startedByUser = false)
        {
            try
            {
                StartLoadingOperation(loadingMessage);
                await func();
                StopLoadingOperation();
            }
            catch (Exception e)
            {
                StopLoadingOperation();
                HandleException(e, startedByUser);
            }
        }

        internal async void HandleException(Exception exception, bool startedByUser = false)
        {

            var errorMessage = ExceptionHandlerService.Instance.Handle(exception);


#if DEBUG

            MessageBoxService.Show(exception.ToString());
            return;
#endif
            
            if (startedByUser)
            {
                try
                {
                    await MessageBoxService.ShowAsync(errorMessage, AppResources.App_Error);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ShowErrorMessage(errorMessage);
            }
             
        }

        internal ICommand CreateSafeCommand(Func<Task> func)
        {
            return new RelayCommand(() => SafeRun(func));
        }

        internal ICommand CreateSafeCommand<T>(Func<T, Task> func)
        {
            return new RelayCommand<T>((p) => SafeRun(() => func(p)));
        }

        internal ICommand CreateLoadingCommand(Func<Task> func, string loadingMessage, bool userInitiated)
        {
            return new RelayCommand(() => LoadingSafeRun(func, loadingMessage, userInitiated));
        }

        internal IAsyncCommand CreateLoadingAsyncCommand(Func<Task> func, string loadingMessage, bool userInitiated)
        {
            return new AsyncCommand(() => LoadingSafeRun(func, loadingMessage, userInitiated));
        }

        internal ICommand CreateLoadingCommand<T>(Func<T, Task> func, string loadingMessage, bool userInitiated)
        {
            return new RelayCommand<T>(p => LoadingSafeRun(() => func(p), loadingMessage, userInitiated));
        }

        internal ICommand CreateCommandForUserAction(Func<Task> func, string loadingMessage)
        {
            return CreateLoadingCommand(func, loadingMessage, true);
        }

        internal IAsyncCommand CreateAsyncCommandForUserAction(Func<Task> func, string loadingMessage)
        {
            return CreateLoadingAsyncCommand(func, loadingMessage, true);
        }

        internal ICommand CreateCommandForUserAction<T>(Func<T, Task> func, string loadingMessage)
        {
            return CreateLoadingCommand(func, loadingMessage, true);
        }

        internal ICommand CreateCommand<T>(Func<T, Task> func, string loadingMessage)
        {
            return CreateLoadingCommand(func, loadingMessage, false);
        }

        internal ICommand CreateCommand(Func<Task> func, string loadingMessage)
        {
            return CreateLoadingCommand(func, loadingMessage, false);
        }

        internal async void ShowErrorMessage(string message)
        {
            GenericMessage = message;
            await Task.Delay(ShowErrorTime);
            GenericMessage = null;
        }

        internal void StartLoadingOperation(string loadingMessage)
        {
            IsLoadingOperationRunning = true;
            GenericMessage = loadingMessage;
        }

        internal void StopLoadingOperation()
        {
            IsLoadingOperationRunning = false;
            GenericMessage = null;
        }


        /// <summary>
        /// The <see cref="GenericMessage" /> property's name.
        /// </summary>
        public const string GenericMessagePropertyName = "GenericMessage";

        private string _genericMessage;

        /// <summary>
        /// Sets and gets the GenericMessage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string GenericMessage
        {
            get
            {
                return _genericMessage;
            }

            set
            {
                if (_genericMessage == value)
                {
                    return;
                }

                _genericMessage = value;
                RaisePropertyChanged(GenericMessagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsLoadingOperationRunning" /> property's name.
        /// </summary>
        public const string IsLoadingOperationRunningPropertyName = "IsLoadingOperationRunning";

        private bool _isLoadingOperationRunning = false;

        /// <summary>
        /// Sets and gets the IsLoadingOperationRunning property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsLoadingOperationRunning
        {
            get
            {
                return _isLoadingOperationRunning;
            }

            set
            {
                if (_isLoadingOperationRunning == value)
                {
                    return;
                }

                _isLoadingOperationRunning = value;
                RaisePropertyChanged(IsLoadingOperationRunningPropertyName);
            }
        }


        public void Save()
        {
            try
            {
                using (var storage = AppCommonServicesContainer.Instance.OpenStorageForState())
                {
                    Save(storage);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#endif
            }
        }

        public void Load()
        {
            try
            {
                using (var storage = AppCommonServicesContainer.Instance.OpenStorageForState())
                {
                    Load(storage);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#endif
            }
        }
        protected abstract void Save(IStorage storage);
        protected abstract void Load(IStorage storage);
    }

}
