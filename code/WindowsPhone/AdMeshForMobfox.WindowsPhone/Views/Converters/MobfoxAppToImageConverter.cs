using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AdMesh.Service.MobFox.Data;

namespace AdMeshForMobfox.WindowsPhone.Views.Converters
{
    public class MobfoxAppToImageConverter : IValueConverter
    {
        private const string BasePath = "../Assets/Images/Platforms/";
        private const string AndroidImage = BasePath + "Android.png";
        private const string WindowsPhoneImage = BasePath + "WindowsPhone.png";
        private const string IosImage = BasePath + "iOs.png";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var app = value as MobfoxApplication;
            switch (app.Type)
            {
                case MobfoxApplication.Androidpp:
                    return AndroidImage;
                case MobfoxApplication.WindowsPhoneApp:
                    return WindowsPhoneImage;
                default:
                    return IosImage;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
