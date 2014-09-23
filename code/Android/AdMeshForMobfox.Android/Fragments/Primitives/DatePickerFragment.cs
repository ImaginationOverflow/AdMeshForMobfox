using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdMeshForMobfox.Android.Fragments.Primitives
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public DateTime StartDate { get; set; }
        public event EventHandler<DateTime> DateSelected;

       
        public DatePickerFragment(DateTime startDate)
        {
            StartDate = startDate;
        }

        public DatePickerFragment()
        {
            StartDate = DateTime.Now;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new DatePickerDialog(Activity, this, StartDate.Year, StartDate.Month-1, StartDate.Day);
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            OnDateSelected( new DateTime(year, monthOfYear + 1, dayOfMonth));
        }

        protected virtual void OnDateSelected(DateTime e)
        {
            EventHandler<DateTime> handler = DateSelected;
            if (handler != null) handler(this, e);
        }


    }
}