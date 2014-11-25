using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AdMesh.ViewModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Android.Ui.Primitives;
using IOToolkit.Services;
using Java.Net;

namespace AdMeshForMobfox.Android.Fragments
{
    public class LoginFragment : BaseFragment<LoginViewModel>
    {
        public LoginFragment()
            : base(Resource.Layout.LoginLayout)
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }


        protected override void OnPrepareBindings(View view)
        {
            var button = view.FindViewById<Button>(Resource.Id.login_button);
            var email = view.FindViewById<EditText>(Resource.Id.login_email);
            var pwd = view.FindViewById<EditText>(Resource.Id.login_password);

            button.SetBackgroundDrawable(AppResources.GetDrawable(Resource.Drawable.login_btn_selector));
            button.Click +=  (s, a) =>
            {
                ViewModel.Email = email.Text;
                ViewModel.Password = pwd.Text;
                ViewModel.LoginCommand.Execute(null);
            };
        }

        void button_Click(object sender, EventArgs e)
        {
        }
    }
}