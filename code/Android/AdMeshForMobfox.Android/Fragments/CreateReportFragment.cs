using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdMesh.Localisation;
using AdMesh.ViewModel;
using AdMeshForMobfox.Android.DumbBinding;
using AdMeshForMobfox.Android.Fragments.Adapters;
using AdMeshForMobfox.Android.Fragments.Helpers;
using AdMeshForMobfox.Android.Fragments.Primitives;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments
{
    internal class CreateReportFragment : BaseDialogFragment<ReportViewModel>
    {
        private DatePickerFragment _dateTimePickerFragment;

        public DatePickerFragment DateTimePicker
        {
            get { return _dateTimePickerFragment ?? (_dateTimePickerFragment = new DatePickerFragment()); }
            set { _dateTimePickerFragment = value; }
        }

        public CreateReportFragment()
            : base(Resource.Layout.CustomReportLayout)
        {
        }

       

        public override void OnDismiss(IDialogInterface dialog)
        {
            ViewModel.ClearCommand.Execute(null);
        }

   
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            return dialog;
        }

        protected override void OnPrepareBindings(View view)
        {
            base.OnPrepareBindings(view);

            var appSpinner = view.FindViewById<Spinner>(Resource.Id.custom_report_app);
            var endDateContainer = view.FindViewById<LinearLayout>(Resource.Id.create_report_end_date_container);
            var startDateContainer = view.FindViewById<LinearLayout>(Resource.Id.create_report_start_date_container);
            var endDate = view.FindViewById<TextView>(Resource.Id.create_report_end_date);
            var startDate = view.FindViewById<TextView>(Resource.Id.create_report_start_date);
            var seeReportBtn = view.FindViewById<Button>(Resource.Id.create_report_btn);
            var checkbox = view.FindViewById<CheckBox>(Resource.Id.create_report_checkbox);


            appSpinner.Adapter = new MobfoxAppSelectionAdapter(Activity, Resource.Layout.AppPickerLayout,
                ViewModel.ApplicationsController.Applications);

            appSpinner.ItemSelected += OnAppSelectionChanged;
            appSpinner.SetSelection(ViewModel.ApplicationsController.Applications.Count, true);


            var vmBinding = this.CreateBindingChain(e => e.ViewModel);

            vmBinding.CreateBinding(vm => vm.StartDate).BindTo(startDate, date => date.ToDateString());
            vmBinding.CreateBinding(vm => vm.EndDate).BindTo(endDate, date => date.ToDateString());

            startDateContainer.Click +=
                (s, a) => HandleDateTimePicking(ViewModel.StartDate, (d) => ViewModel.StartDate = d);

            endDateContainer.Click +=
               (s, a) => HandleDateTimePicking(ViewModel.EndDate, (d) => ViewModel.EndDate = d);

            checkbox.Click += (s, a) =>
            {
                startDateContainer.Enabled = endDateContainer.Enabled = !checkbox.Checked;
                ViewModel.IsOverall = checkbox.Checked;
            };

            seeReportBtn.Click += async (s, a) =>
            {
                await ViewModel.LoadReportCommand.ExecuteAsync();
                Dismiss();
            };
        }

        private void HandleDateTimePicking(DateTime date, Action<DateTime> callback)
        {

            EventHandler<DateTime> handler = null;

            handler = (s, a) =>
            {
                callback(a);
                DateTimePicker.DateSelected -= handler;
            };

            DateTimePicker.DateSelected += handler;
            DateTimePicker.StartDate = date;
            DateTimePicker.Show(FragmentManager, "picker");

        }

        void OnAppSelectionChanged(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var adapter = spinner.Adapter as MobfoxAppSelectionAdapter;
            var app = adapter.GetItem(e.Position);


            ViewModel.Application = string.IsNullOrEmpty(app.Id) ? null : app;


        }
    }
}