using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdMesh.Service.MobFox.Data
{
    [XmlRoot("response")]
    public class MobfoxSessionData : MobfoxDataBase
    {
        [XmlElement("api_account_id")]
        public string ApiAccountId { get; set; }
        [XmlElement("api_key")]
        public string ApiKey { get; set; }
    }
}
