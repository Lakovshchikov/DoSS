using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Doss.Model_Forms_Place
{
    class Form_Place
    { 
    [JsonProperty("currentVersion")]
    public double CurrentVersion { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("geometryType")]
    public string GeometryType { get; set; }

    [JsonProperty("copyrightText")]
    public string CopyrightText { get; set; }

    [JsonProperty("parentLayer")]
    public object ParentLayer { get; set; }

    [JsonProperty("subLayers")]
    public object[] SubLayers { get; set; }

    [JsonProperty("minScale")]
    public long MinScale { get; set; }

    [JsonProperty("maxScale")]
    public long MaxScale { get; set; }

    [JsonProperty("drawingInfo")]
    public DrawingInfo DrawingInfo { get; set; }

    [JsonProperty("defaultVisibility")]
    public bool DefaultVisibility { get; set; }

    [JsonProperty("extent")]
    public Extent Extent { get; set; }

    [JsonProperty("hasAttachments")]
    public bool HasAttachments { get; set; }

    [JsonProperty("htmlPopupType")]
    public string HtmlPopupType { get; set; }

    [JsonProperty("displayField")]
    public string DisplayField { get; set; }

    [JsonProperty("typeIdField")]
    public object TypeIdField { get; set; }

    [JsonProperty("fields")]
    public Field[] Fields { get; set; }

    [JsonProperty("relationships")]
    public object[] Relationships { get; set; }

    [JsonProperty("canModifyLayer")]
    public bool CanModifyLayer { get; set; }

    [JsonProperty("canScaleSymbols")]
    public bool CanScaleSymbols { get; set; }

    [JsonProperty("hasLabels")]
    public bool HasLabels { get; set; }

    [JsonProperty("capabilities")]
    public string Capabilities { get; set; }

    [JsonProperty("maxRecordCount")]
    public long MaxRecordCount { get; set; }

    [JsonProperty("supportsStatistics")]
    public bool SupportsStatistics { get; set; }

    [JsonProperty("supportsAdvancedQueries")]
    public bool SupportsAdvancedQueries { get; set; }

    [JsonProperty("supportedQueryFormats")]
    public string SupportedQueryFormats { get; set; }

    [JsonProperty("ownershipBasedAccessControlForFeatures")]
    public OwnershipBasedAccessControlForFeatures OwnershipBasedAccessControlForFeatures { get; set; }

    [JsonProperty("useStandardizedQueries")]
    public bool UseStandardizedQueries { get; set; }

    [JsonProperty("advancedQueryCapabilities")]
    public AdvancedQueryCapabilities AdvancedQueryCapabilities { get; set; }
}

public partial class AdvancedQueryCapabilities
{
    [JsonProperty("useStandardizedQueries")]
    public bool UseStandardizedQueries { get; set; }

    [JsonProperty("supportsStatistics")]
    public bool SupportsStatistics { get; set; }

    [JsonProperty("supportsOrderBy")]
    public bool SupportsOrderBy { get; set; }

    [JsonProperty("supportsDistinct")]
    public bool SupportsDistinct { get; set; }

    [JsonProperty("supportsPagination")]
    public bool SupportsPagination { get; set; }

    [JsonProperty("supportsTrueCurve")]
    public bool SupportsTrueCurve { get; set; }

    [JsonProperty("supportsReturningQueryExtent")]
    public bool SupportsReturningQueryExtent { get; set; }

    [JsonProperty("supportsQueryWithDistance")]
    public bool SupportsQueryWithDistance { get; set; }
}

public partial class DrawingInfo
{
    [JsonProperty("renderer")]
    public Renderer Renderer { get; set; }

    [JsonProperty("transparency")]
    public long Transparency { get; set; }

    [JsonProperty("labelingInfo")]
    public object LabelingInfo { get; set; }
}

public partial class Renderer
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("field1")]
    public string Field1 { get; set; }

    [JsonProperty("field2")]
    public object Field2 { get; set; }

    [JsonProperty("field3")]
    public object Field3 { get; set; }

    [JsonProperty("fieldDelimiter")]
    public string FieldDelimiter { get; set; }

    [JsonProperty("defaultSymbol")]
    public object DefaultSymbol { get; set; }

    [JsonProperty("defaultLabel")]
    public object DefaultLabel { get; set; }

    [JsonProperty("uniqueValueInfos")]
    public UniqueValueInfo[] UniqueValueInfos { get; set; }
}

public partial class UniqueValueInfo
{
    [JsonProperty("symbol")]
    public Symbol Symbol { get; set; }

    [JsonProperty("value")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Value { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

public partial class Symbol
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("style")]
    public string Style { get; set; }

    [JsonProperty("color")]
    public long[] Color { get; set; }

    [JsonProperty("outline", NullValueHandling = NullValueHandling.Ignore)]
    public Symbol Outline { get; set; }

    [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
    public double? Width { get; set; }
}

    public partial class Extent
{
    [JsonProperty("xmin")]
    public double Xmin { get; set; }

    [JsonProperty("ymin")]
    public double Ymin { get; set; }

    [JsonProperty("xmax")]
    public double Xmax { get; set; }

    [JsonProperty("ymax")]
    public double Ymax { get; set; }

    [JsonProperty("spatialReference")]
    public SpatialReference SpatialReference { get; set; }
}

public partial class SpatialReference
{
    [JsonProperty("wkid")]
    public long Wkid { get; set; }

    [JsonProperty("latestWkid")]
    public long LatestWkid { get; set; }
}

public partial class Field
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("alias")]
    public string Alias { get; set; }

    [JsonProperty("domain")]
    public Domain Domain { get; set; }

    [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
    public long? Length { get; set; }
}

public partial class Domain
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("codedValues")]
    public CodedValue[] CodedValues { get; set; }
}

public partial class CodedValue
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("code")]
    public long Code { get; set; }
}

public partial class OwnershipBasedAccessControlForFeatures
{
    [JsonProperty("allowOthersToQuery")]
    public bool AllowOthersToQuery { get; set; }
}

public partial class FormPlace
{
    public static FormPlace FromJson(string json) => JsonConvert.DeserializeObject<FormPlace>(json, Model_Forms_Place.Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this FormPlace self) => JsonConvert.SerializeObject(self, Model_Forms_Place.Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class ParseStringConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        long l;
        if (Int64.TryParse(value, out l))
        {
            return l;
        }
        throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}
}
