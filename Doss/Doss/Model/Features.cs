using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Doss.Model
{
    public partial class Features
    {
        [JsonProperty("features")]
        public Feature[] FeaturesFeatures { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("attrs")]
        public Attrs Attrs { get; set; }

        [JsonProperty("center")]
        public Center Center { get; set; }

        [JsonProperty("extent")]
        public Extent Extent { get; set; }

        [JsonProperty("sort")]
        public long Sort { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }
    }

    public partial class Attrs
    {
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("cn")]
        public string Cn { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("kvartal", NullValueHandling = NullValueHandling.Ignore)]
        public string Kvartal { get; set; }

        [JsonProperty("sn_cn", NullValueHandling = NullValueHandling.Ignore)]
        public string SnCn { get; set; }
    }

    public partial class Center
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public partial class Extent
    {
        [JsonProperty("xmax")]
        public double Xmax { get; set; }

        [JsonProperty("xmin")]
        public double Xmin { get; set; }

        [JsonProperty("ymax")]
        public double Ymax { get; set; }

        [JsonProperty("ymin")]
        public double Ymin { get; set; }
    }

}
