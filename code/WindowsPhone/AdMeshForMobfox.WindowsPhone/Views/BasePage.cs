using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AdMesh.ViewModel;

namespace AdMeshForMobfox.WindowsPhone.Views
{
    public class BasePage : Page
    {
        private readonly Color _barBackgroundColor = Colors.Transparent;
        private readonly Color _barForegroundColor = Color.FromArgb(255, 254, 255, 255);
        private StatusBar _bar;

        public BasePage()
            : this(Colors.Transparent)
        {
        }


        public BasePage(Color foregroundColor)
        {
            _barBackgroundColor = foregroundColor;
        }

        public BasePage(bool usePhoneAccentBrush)
            : this(((SolidColorBrush)Application.Current.Resources["AccentBrush"]).Color)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Load();
        }

        private async void Load()
        {
            _bar = StatusBar.GetForCurrentView();
            _bar.BackgroundOpacity = 1;
            _bar.BackgroundColor = _barBackgroundColor;
            _bar.ForegroundColor = _barForegroundColor;

            if (!(DataContext is AppViewModelBase))
                return;

            var vm = ((AppViewModelBase)DataContext);

            vm.PropertyChanged -= vm_PropertyChanged;
            vm.PropertyChanged += vm_PropertyChanged;
            _bar.ShowAsync();

            if (vm.IsLoadingOperationRunning || string.IsNullOrEmpty(vm.GenericMessage) == false)
            {
                vm_PropertyChanged(this, new PropertyChangedEventArgs(AppViewModelBase.GenericMessagePropertyName));
            }


        }

       
        void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = GetViewModel<AppViewModelBase>();

            if (e.PropertyName.Equals(AppViewModelBase.IsLoadingOperationRunningPropertyName))
            {
                if (vm.IsLoadingOperationRunning)
                {
                    // Loading operation is starting wait for the set on the generic message.
                    return;
                }
                else
                {
                    //
                    //  Progress ended
                    //
                    _bar.ProgressIndicator.HideAsync();
                    return;
                }
            }

            if (e.PropertyName == AppViewModelBase.GenericMessagePropertyName)
            {
                if (string.IsNullOrEmpty(vm.GenericMessage) == false)
                {
                    _bar.ProgressIndicator.Text = vm.GenericMessage;
                    _bar.ProgressIndicator.ShowAsync();
                }
                else
                {
                    _bar.ProgressIndicator.HideAsync();
                }
            }

        }

    
        protected T GetViewModel<T>()
        {
            return (T)DataContext;
        }




    }
}
