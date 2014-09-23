using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using AdMesh.Storage;
using GalaSoft.MvvmLight;
using IOToolkit.Controllers;
using IOToolkit.Helpers;
using IOToolkit.Storage;

namespace AdMesh.Controllers
{
    public class AccountController : ObservableObject, IDataController
    {
        private readonly IMobfoxService _mobfoxService;
        private readonly MobfoxSessionStorage _storage;

        /// <summary>
        /// The <see cref="Session" /> property's name.
        /// </summary>
        public const string SessionPropertyName = "Session";

        private MobfoxSessionData _session;

        /// <summary>
        /// Sets and gets the Session property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MobfoxSessionData Session
        {
            get
            {
                return _session;
            }

            set
            {
                if (_session == value)
                {
                    return;
                }

                _session = value;
                RaisePropertyChanged(SessionPropertyName);
            }
        }


        public AccountController(IMobfoxService mobfoxService, MobfoxSessionStorage storage)
        {
            _mobfoxService = mobfoxService;
            _storage = storage;
        }

        public async Task LoginAsync(string email, string password)
        {
            Session = await _mobfoxService.LoginAsync(email, password);
        }

        public async Task Load()
        {
            Session = await _storage.Load();
            await _mobfoxService.ConnectAsync(Session);
        }

        public async Task Save()
        {
            if (Session == null)
                return;

            await _storage.Save(Session);
        }
    }
}
