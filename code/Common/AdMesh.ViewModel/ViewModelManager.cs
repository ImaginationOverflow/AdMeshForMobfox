using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Services;
using Microsoft.Practices.ServiceLocation;

namespace AdMesh.ViewModel
{
    public class ViewModelManager
    {
        public static void ConfigureViewModels()
        {
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<ApplicationViewModel>();
        }
        
        public static Task RestoreViewModels()
        {
            return Task.FromResult(true);
        }


        public static object GetViewModel(Type t)
        {
            return ServiceLocator.Current.GetInstance(t);
        }

        public static T GetViewModel<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }

        public LoginViewModel Login { get { return GetViewModel<LoginViewModel>(); } }
        public HomeViewModel Home { get { return GetViewModel<HomeViewModel>(); } }
        public ReportViewModel Report { get { return GetViewModel<ReportViewModel>(); } }
        public ApplicationViewModel Application { get { return GetViewModel<ApplicationViewModel>(); } }
        


        public static void SaveState()
        {
            HandleSaveState<HomeViewModel>();
        }


        public static void RestoreState()
        {
            RestoreSavedState<HomeViewModel>();
        }

        private static void HandleSaveState<T>() where T : AppViewModelBase
        {
            if (SimpleIoc.Default.ContainsCreated<T>())
                SimpleIoc.Default.GetInstance<T>().Save();
        }

        private static void RestoreSavedState<T>() where T : AppViewModelBase
        {
            SimpleIoc.Default.GetInstance<T>().Load();
        }

        public static void ClearSavedState()
        {
            AppCommonServicesContainer.Instance.DeleteTransistentStorage();
        }
    }
}
