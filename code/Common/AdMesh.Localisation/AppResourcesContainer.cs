using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdMesh.Localisation
{
    public class AppResourcesContainer
    {
        public AppResources Strings { get; private set; }

        public AppResourcesContainer()
        {
            Strings = new AppResources();
        }
    }
}
