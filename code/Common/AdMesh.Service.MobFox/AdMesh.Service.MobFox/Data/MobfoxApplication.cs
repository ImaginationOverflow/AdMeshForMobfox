using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdMesh.Service.MobFox.Data
{

    [XmlRoot("response")]
    public class MobfoxApplicationResponse : MobfoxDataBase
    {

        public MobfoxApplicationResponse()
        {
            Applications = new List<MobfoxApplication>();
        }

        [XmlElement(ElementName = "publication")]
        public List<MobfoxApplication> Applications { get; set; }
    }

    [XmlRoot("publication")]
    public class MobfoxApplication : MobfoxDataBase
    {
        public const string IosApp = "iphone_app";
        public const string WindowsPhoneApp = "wp7_app";
        public const string Androidpp = "android_app";

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }


        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
