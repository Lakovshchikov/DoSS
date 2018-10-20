using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Doss.Model_Type_Of_Use
{
    class TypesOf_Use
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
        public DefaultSymbol DefaultSymbol { get; set; }

        [JsonProperty("defaultLabel")]
        public string DefaultLabel { get; set; }

        [JsonProperty("uniqueValueInfos")]
        public UniqueValueInfo[] UniqueValueInfos { get; set; }
    }

    public partial class DefaultSymbol
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("style")]
        public Style Style { get; set; }

        [JsonProperty("color")]
        public long[] Color { get; set; }

        [JsonProperty("outline", NullValueHandling = NullValueHandling.Ignore)]
        public DefaultSymbol Outline { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public double? Width { get; set; }
    }

    public partial class UniqueValueInfo
    {
        [JsonProperty("symbol")]
        public DefaultSymbol Symbol { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
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
        public Code Code { get; set; }
    }

    public partial class OwnershipBasedAccessControlForFeatures
    {
        [JsonProperty("allowOthersToQuery")]
        public bool AllowOthersToQuery { get; set; }
    }

    public enum Style { EsriSfsSolid, EsriSlsSolid };

    public enum TypeEnum { EsriSfs, EsriSls };

    public partial struct Code
    {
        public long? Integer;
        public string String;

        public static implicit operator Code(long Integer) => new Code { Integer = Integer };
        public static implicit operator Code(string String) => new Code { String = String };
    }

    public partial class TypesOfUse
    {
        public static TypesOfUse FromJson(string json) => JsonConvert.DeserializeObject<TypesOfUse>(json, Model_Type_Of_Use.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TypesOfUse self) => JsonConvert.SerializeObject(self, Model_Type_Of_Use.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                StyleConverter.Singleton,
                TypeEnumConverter.Singleton,
                CodeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StyleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Style) || t == typeof(Style?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "esriSFSSolid":
                    return Style.EsriSfsSolid;
                case "esriSLSSolid":
                    return Style.EsriSlsSolid;
            }
            throw new Exception("Cannot unmarshal type Style");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Style)untypedValue;
            switch (value)
            {
                case Style.EsriSfsSolid:
                    serializer.Serialize(writer, "esriSFSSolid");
                    return;
                case Style.EsriSlsSolid:
                    serializer.Serialize(writer, "esriSLSSolid");
                    return;
            }
            throw new Exception("Cannot marshal type Style");
        }

        public static readonly StyleConverter Singleton = new StyleConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "esriSFS":
                    return TypeEnum.EsriSfs;
                case "esriSLS":
                    return TypeEnum.EsriSls;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.EsriSfs:
                    serializer.Serialize(writer, "esriSFS");
                    return;
                case TypeEnum.EsriSls:
                    serializer.Serialize(writer, "esriSLS");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

    internal class CodeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Code) || t == typeof(Code?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Code { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Code { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Code");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Code)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Code");
        }

        public static readonly CodeConverter Singleton = new CodeConverter();
    }
}
