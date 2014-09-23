using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdMesh.Service.MobFox.Data
{
    [XmlRoot(ElementName = "response")]
    public class MobfoxReportResponse : MobfoxSessionData
    {
        [XmlElement("report")]
        public MobfoxReportInternal ReportInternal { get; set; }
    }

    public class MobfoxReportInternal
    {
        [XmlElement("statistics")]
        public MobfoxReport Statistics { get; set; }
    }

    public class MobfoxMonetaryValue
    {
        [XmlElement("currency")]
        public string Currency { get; set; }
        [XmlElement("amount")]
        public Double Amount { get; set; }
    }

    public class MobfoxReport
    {

        public bool IsOverallReport { get { return StartTime ==null && EndTime == null; } }
        public bool IsTodayReport { get
        {
            return StartTime != null && EndTime != null && StartTime.Value == EndTime.Value;
        } }
        
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [XmlElement("impressions")]
        public long Impressions { get; set; }

        [XmlElement("clicks")]
        public long Clicks { get; set; }

        [XmlElement("click_through_rate")]
        public double Ctr { get; set; }

        [XmlElement("average_cpc")]
        public MobfoxMonetaryValue AverageCpc { get; set; }

        [XmlElement("total_earnings")]
        public MobfoxMonetaryValue Earnings { get; set; }

        [XmlElement("effective_cpm")]
        public MobfoxMonetaryValue Ecpm { get; set; }
    }
}
