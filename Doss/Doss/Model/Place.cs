using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Doss.Model2
{
    public class Place
    {
        [JsonProperty("feature")]
        public Feature Feature { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("attrs")]
        public Attrs Attrs { get; set; }

        [JsonProperty("center")]
        public Center Center { get; set; }

        [JsonProperty("extent")]
        public Extent Extent { get; set; }

        [JsonProperty("stat")]
        public Stat Stat { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }
    }

    public partial class Attrs
    {
        [JsonProperty("adate")]
        public string Adate { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("anno_text")]
        public long AnnoText { get; set; }

        [JsonProperty("area_type")]
        public string AreaType { get; set; }

        [JsonProperty("area_unit")]
        public string AreaUnit { get; set; }

        [JsonProperty("area_value")]
        public long AreaValue { get; set; }

        [JsonProperty("cad_cost")]
        public string CadCost { get; set; }

        [JsonProperty("cad_eng_data")]
        public CadEngData CadEngData { get; set; }

        [JsonProperty("cad_record_date")]
        public string CadRecordDate { get; set; }

        [JsonProperty("cad_unit")]
        public long CadUnit { get; set; }

        [JsonProperty("category_type")]
        public string CategoryType { get; set; }

        [JsonProperty("cn")]
        public string Cn { get; set; }

        [JsonProperty("date_cost")]
        public string DateCost { get; set; }

        [JsonProperty("date_create")]
        public string DateCreate { get; set; }

        [JsonProperty("fp")]
        public string Fp { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("kvartal")]
        public string Kvartal { get; set; }

        [JsonProperty("kvartal_cn")]
        public string KvartalCn { get; set; }

        [JsonProperty("okrug")]
        public long Okrug { get; set; }

        [JsonProperty("okrug_cn")]
        public long OkrugCn { get; set; }

        [JsonProperty("pubdate")]
        public string Pubdate { get; set; }

        [JsonProperty("rayon")]
        public string Rayon { get; set; }

        [JsonProperty("rayon_cn")]
        public string RayonCn { get; set; }

        [JsonProperty("reg")]
        public long Reg { get; set; }

        [JsonProperty("rifr")]
        public object Rifr { get; set; }

        [JsonProperty("rights_reg")]
        public long RightsReg { get; set; }

        [JsonProperty("sale")]
        public object Sale { get; set; }

        [JsonProperty("statecd")]
        public string Statecd { get; set; }

        [JsonProperty("util_by_doc")]
        public string UtilByDoc { get; set; }

        [JsonProperty("util_code")]
        public string UtilCode { get; set; }
    }

    public partial class CadEngData
    {
        [JsonProperty("actual_date")]
        public string ActualDate { get; set; }

        [JsonProperty("co_name")]
        public string CoName { get; set; }

        [JsonProperty("lastmodified")]
        public string Lastmodified { get; set; }

        [JsonProperty("rc_type")]
        public long RcType { get; set; }
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

    public partial class Stat
    {
    }

}