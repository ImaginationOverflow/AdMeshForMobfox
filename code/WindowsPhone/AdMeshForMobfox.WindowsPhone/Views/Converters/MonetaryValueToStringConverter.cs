using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AdMesh.Common;
using AdMesh.Service.MobFox.Data;

namespace AdMeshForMobfox.WindowsPhone.Views.Converters
{
    public class MonetaryValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (value as MobfoxMonetaryValue);

            return v.ToPrettyString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
