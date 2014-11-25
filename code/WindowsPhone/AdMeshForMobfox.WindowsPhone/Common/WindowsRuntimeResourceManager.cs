using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace AdMeshForMobfox.WindowsPhone.Common
{
    /// <summary>
    /// Visit http://blogs.msdn.com/b/philliphoff/archive/2014/11/19/missingmanifestresourceexception-when-using-portable-class-libraries-in-winrt.aspx for futher explanation
    /// </summary>
    public class WindowsRuntimeResourceManager : ResourceManager
    {
        private readonly ResourceLoader resourceLoader;

        private WindowsRuntimeResourceManager(string baseName, Assembly assembly)
            : base(baseName, assembly)
        {
            this.resourceLoader = ResourceLoader.GetForViewIndependentUse(baseName);
        }

        public static void InjectIntoResxGeneratedApplicationResourcesClass(Type resxGeneratedApplicationResourcesClass)
        {
            resxGeneratedApplicationResourcesClass.GetRuntimeFields()
              .First(m => m.Name == "resourceMan")
              .SetValue(null, new WindowsRuntimeResourceManager(resxGeneratedApplicationResourcesClass.FullName, resxGeneratedApplicationResourcesClass.GetTypeInfo().Assembly));
        }

        public override string GetString(string name, CultureInfo culture)
        {
                return this.resourceLoader.GetString(name);
        }
    }
}
