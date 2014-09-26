using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AdMesh.Common;
using AdMesh.Controllers;
using AdMesh.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Controllers;
using IOToolkit.Helpers;
using IOToolkit.Services;
using IOToolkit.Storage;

namespace AdMesh.Configurator
{
    //TODO move this
    public abstract class AppManagerBase
    {
        const string ExceptionTag = "excep";
        private ControllerCollection _controllers;

        public IAppNavigationService NavigationService { get { return SimpleIoc.Default.GetInstance<IAppNavigationService>(); } }

        public virtual void PreInitialize()
        {

        }

        public async Task Initialize()
        {

            await PlatformSetup();

            try
            {
                _controllers = SimpleIoc.Default.GetInstance<ControllerCollection>();
                _controllers.Initialize();
                foreach (var controller in _controllers)
                {

                    await controller.Load();

                }
            }
            catch (Exception e)
            {
#if DEBUG
                if(Debugger.IsAttached)
                    Debugger.Break();
                SimpleIoc.Default.GetInstance<IMessageBoxService>().ShowAsync(e.ToString(), "Loading");
                return;
#endif
                throw;
            }
        }

        public async Task Activated(bool dataPreserved)
        {
            if (dataPreserved)
            {
                ViewModelManager.ClearSavedState();
                return;
            }

            PreInitialize();
            await Initialize();
            ViewModelManager.RestoreState();
            //Call start?
        }

        public Task Deactivated()
        {
            return SaveData(true);
        }



        public Task Closing()
        {
            return SaveData();
        }

        private async Task SaveData(bool saveViewModelState = false)
        {
            try
            {
                foreach (var controller in _controllers)
                {
                    await controller.Save();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                SimpleIoc.Default.GetInstance<IMessageBoxService>().ShowAsync(e.ToString(), "Loading");
                return;
#endif
                throw;
            }

            if (saveViewModelState)
                ViewModelManager.SaveState();

        }


        public void Start()
        {
            var accountController = SimpleIoc.Default.GetInstance<AccountController>();

            if (accountController.Session != null)
            {
                NavigationService.GoToHome();
                return;
            }



            NavigationService.GoToLogin();

        }

        protected virtual Task PlatformSetup()
        {
            SimpleIoc.Default.Reset();
            CommonConfigurator.Configure();
            return TaskHelpers.FinishTask();
        }

        public void OnUnhandledException(Exception e)
        {

        }

    }
}