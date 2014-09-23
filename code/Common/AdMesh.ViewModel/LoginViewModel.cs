using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdMesh.Common;
using AdMesh.Controllers;
using AdMesh.Localisation;
using IOToolkit.Storage;

namespace AdMesh.ViewModel
{
    public class LoginViewModel : AppViewModelBase
    {
        private readonly IAppNavigationService _navigationService;
        public AccountController AccountController { get; private set; }

        /// <summary>
        /// The <see cref="Email" /> property's name.
        /// </summary>
        public const string EmailPropertyName = "Email";

        private string _email = "email";

        /// <summary>
        /// Sets and gets the Email property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (_email == value)
                {
                    return;
                }

                _email = value;
                RaisePropertyChanged(EmailPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        private string _pwd = string.Empty;

        /// <summary>
        /// Sets and gets the Password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Password
        {
            get
            {
                return _pwd;
            }

            set
            {
                if (_pwd == value)
                {
                    return;
                }

                _pwd = value;
                RaisePropertyChanged(PasswordPropertyName);
            }
        }

        public LoginViewModel(AccountController accountController, IAppNavigationService navigationService)
        {
            _navigationService = navigationService;
            AccountController = accountController;
        }


        private ICommand _loginCommand;

        /// <summary>
        /// Gets the LoginCommand.
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand 
                    ?? (_loginCommand = CreateCommandForUserAction(
                                          async() =>
                                          {
                                              if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
                                              {
                                                  await MessageBoxService.ShowAsync(AppResources.Login_Form_Data_Incorrect,AppResources.App_Error);
                                                  return;
                                              }

                                              await AccountController.LoginAsync(Email, Password);
                                              _navigationService.GoToHome();

                                          },AppResources.App_Loading));
										    
            }
        }

        protected override void Save(IStorage storage)
        {
        }

        protected override void Load(IStorage storage)
        {
        }
    }
}
