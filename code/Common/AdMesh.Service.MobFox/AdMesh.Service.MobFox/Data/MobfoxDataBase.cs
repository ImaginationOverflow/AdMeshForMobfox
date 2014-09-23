using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdMesh.Service.MobFox.Data
{
    public class MobfoxDataBase
    {
        [XmlElement("error")]
        public string Error { get; set; }
    }
}
