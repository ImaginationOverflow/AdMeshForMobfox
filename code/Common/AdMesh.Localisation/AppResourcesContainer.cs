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
        private static readonly AppResources Resources = new AppResources();
        public AppResources Strings { get { return Resources; } }

        public AppResourcesContainer()
        {
        }
    }
}
