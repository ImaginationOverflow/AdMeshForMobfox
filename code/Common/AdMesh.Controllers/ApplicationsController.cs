using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox;
using AdMesh.Service.MobFox.Data;
using AdMesh.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using IOToolkit.Controllers;
using IOToolkit.Helpers;
using IOToolkit.Services;

namespace AdMesh.Controllers
{
    public class ApplicationsController : ObservableObject, IDataController
    {
        private readonly IMobfoxService _mobfoxService;
        private readonly MobfoxApplicationStorage _storage;

        /// <summary>
        /// The <see cref="Applications" /> property's name.
        /// </summary>
        public const string ApplicationsPropertyName = "Applications";

        private ObservableCollection<MobfoxApplication> _apps = new ObservableCollection<MobfoxApplication>();

        /// <summary>
        /// Sets and gets the Applications property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MobfoxApplication> Applications
        {
            get
            {
                return _apps;
            }

            set
            {
                if (_apps == value)
                {
                    return;
                }

                _apps = value;
                RaisePropertyChanged(ApplicationsPropertyName);
            }
        }

        public ApplicationsController(IMobfoxService mobfoxService, MobfoxApplicationStorage storage)
        {
            _mobfoxService = mobfoxService;
            _storage = storage;
        }

        public async Task LoadApplicationsAsync()
        {
            var apps = await _mobfoxService.GetApplicationsAsync();

            Applications.UpdateWithNew(apps);
        }

        public async Task Load()
        {
            var data = await _storage.Load();

            if (data != null)
                Applications.AddRange(data);

        }

        public  Task Save()
        {
            return _storage.Save(Applications);
        }
    }
}
