using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox.Data;

namespace AdMesh.Common
{
    public static class AppHelpers
    {
        public static string ToPrettyString(this MobfoxMonetaryValue value)
        {
            return string.Format("${0}", value.Amount.ToString("N"));
        }

        public static string ToPercentagePrettyString(this double percentage)
        {
            return string.Format("{0} %", percentage);
        }

    }
}
