using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Phone.Devices.Notification;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using AdMesh.ViewModel;
using AdMeshForMobfox.WindowsPhone.Common;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641
using AdMeshForMobfox.WindowsPhone.Views.Controls;

namespace AdMeshForMobfox.WindowsPhone.Views
{
    public sealed partial class Home : BasePage
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Home()
            : base(true)
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            Loaded += Home_Loaded;
        }

        void Home_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
                GetViewModel<HomeViewModel>().RefreshCommand.Execute(null);
            base.OnNavigatedTo(e);
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

        }

        #endregion

        private void Vibrate(object sender, TappedRoutedEventArgs e)
        {
            VibrationDevice.GetDefault().Vibrate(TimeSpan.FromMilliseconds(50));
        }

        private void OnItemClicked(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if(lv.SelectedIndex == -1)
                return;

            GetViewModel<HomeViewModel>().NavigateToApplicationCommand.Execute(lv.SelectedItem);

            lv.SelectedIndex = -1;

        }

        private async void AddAppBasdarButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CreateReportDialog().ShowAsync();
        }

        private void RateApp(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + Package.Current.Id.Name));
        }
    }
}
