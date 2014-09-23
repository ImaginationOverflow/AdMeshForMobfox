using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdMesh.Configurator.Android
{
    public class Configurator
    {
        public static void Configure()
        {
            CommonConfigurator.Configure();
            IOToolkitConfigurator.Configure();
        }
    }
}
