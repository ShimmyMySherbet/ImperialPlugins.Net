using Newtonsoft.Json;
using System;

// Models Auto-Generated
namespace ImperialPlugins.Models.Har
{
    public partial class HarContent
    {
        public Log Log { get; set; }
    }

    public partial class Log
    {
        public string Version { get; set; }

        public Creator Creator { get; set; }

        //public Page[] Pages { get; set; }

        public Entry[] Entries { get; set; }
    }

    public partial class Creator
    {
        public string Name { get; set; }

        public string Version { get; set; }
    }

    public partial class Entry
    {
        //public Initiator Initiator { get; set; }


        public string ResourceType { get; set; }

        public Cache Cache { get; set; }

        //public Pageref Pageref { get; set; }

        public Request Request { get; set; }

        public Response Response { get; set; }

        //public ServerIpAddress ServerIpAddress { get; set; }

        public DateTimeOffset StartedDateTime { get; set; }

        public double Time { get; set; }

        public Timings Timings { get; set; }

    }

    public partial class Cache
    {
    }

    //public partial class Initiator
    //{
    //    public TypeEnum Type { get; set; }

    //    public Uri Url { get; set; }

    //    public long? LineNumber { get; set; }

    //    public Stack Stack { get; set; }
    //}

    //public partial class Stack
    //{
    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public StackParent Parent { get; set; }
    //}

    //public partial class CallFrame
    //{
    //    [JsonProperty("functionName")]
    //    public string FunctionName { get; set; }

    //    [JsonProperty("scriptId")]
    //    [JsonConverter(typeof(ParseStringConverter))]
    //    public long ScriptId { get; set; }

    //    [JsonProperty("url")]
    //    public string Url { get; set; }

    //    [JsonProperty("lineNumber")]
    //    public long LineNumber { get; set; }

    //    [JsonProperty("columnNumber")]
    //    public long ColumnNumber { get; set; }
    //}

    //public partial class StackParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public PurpleParent Parent { get; set; }
    //}

    //public partial class PurpleParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public FluffyParent Parent { get; set; }
    //}

    //public partial class FluffyParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public TentacledParent Parent { get; set; }
    //}

    //public partial class TentacledParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public StickyParent Parent { get; set; }
    //}

    //public partial class StickyParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
    //    public IndigoParent Parent { get; set; }
    //}

    //public partial class IndigoParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }

    //    [JsonProperty("parent")]
    //    public IndecentParent Parent { get; set; }
    //}

    //public partial class IndecentParent
    //{
    //    [JsonProperty("description")]
    //    public Description Description { get; set; }

    //    [JsonProperty("callFrames")]
    //    public CallFrame[] CallFrames { get; set; }
    //}

    public partial class Request
    {
        //[JsonProperty("method")]
        //public Method Method { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        //[JsonProperty("httpVersion")]
        //public HttpVersion HttpVersion { get; set; }

        [JsonProperty("headers")]
        public Header[] Headers { get; set; }

        [JsonProperty("queryString")]
        public Header[] QueryString { get; set; }

        [JsonProperty("cookies")]
        public Cooky[] Cookies { get; set; }

        [JsonProperty("headersSize")]
        public long HeadersSize { get; set; }

        [JsonProperty("bodySize")]
        public long BodySize { get; set; }
    }

    public partial class Cooky
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("expires")]
        public DateTimeOffset Expires { get; set; }

        [JsonProperty("httpOnly")]
        public bool HttpOnly { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Response
    {
        public long Status { get; set; }

        //public StatusText StatusText { get; set; }

        //public HttpVersion HttpVersion { get; set; }

        public Header[] Headers { get; set; }

        public object[] Cookies { get; set; }

        public Content Content { get; set; }

        public string RedirectUrl { get; set; }

        public long HeadersSize { get; set; }

        public long BodySize { get; set; }

        public long TransferSize { get; set; }

        public string Error { get; set; }
    }

    public partial class Content
    {
        public long Size { get; set; }

        public string MimeType { get; set; }
    }

    public partial class Timings
    {
        public double Blocked { get; set; }

        public long Dns { get; set; }

        public long Ssl { get; set; }

        public long Connect { get; set; }

        public double Send { get; set; }

        public double Wait { get; set; }

        public double Receive { get; set; }

    }


    //public partial class PageTimings
    //{
    //    [JsonProperty("onContentLoad")]
    //    public double OnContentLoad { get; set; }

    //    [JsonProperty("onLoad")]
    //    public double OnLoad { get; set; }
    //}

    //public enum FromCache { Disk };

    //public enum Description { Image, Load, PromiseThen };

    //public enum TypeEnum { Other, Parser, Preflight, Script };

    ////public enum Pageref { Page1 };

    //public enum Priority { High, Low, VeryHigh };

    //public enum HttpVersion { H3, Http11, Http20 };

    //public enum Method { Get, Options, Put };

    //public enum StatusText { Empty, InternalRedirect };

    //public enum ServerIpAddress { Empty, The17267220149 };

    public partial class HarContent
    {
        public static HarContent FromJson(string json) => JsonConvert.DeserializeObject<HarContent>(json);
    }

    public static class Serialize
    {
        public static string ToJson(this HarContent self) => JsonConvert.SerializeObject(self);
    }

    //internal class FromCacheConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(FromCache) || t == typeof(FromCache?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        if (value == "disk")
    //        {
    //            return FromCache.Disk;
    //        }
    //        throw new Exception("Cannot unmarshal type FromCache");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (FromCache)untypedValue;
    //        if (value == FromCache.Disk)
    //        {
    //            serializer.Serialize(writer, "disk");
    //            return;
    //        }
    //        throw new Exception("Cannot marshal type FromCache");
    //    }

    //    public static readonly FromCacheConverter Singleton = new FromCacheConverter();
    //}

    //internal class ParseStringConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        long l;
    //        if (Int64.TryParse(value, out l))
    //        {
    //            return l;
    //        }
    //        throw new Exception("Cannot unmarshal type long");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (long)untypedValue;
    //        serializer.Serialize(writer, value.ToString());
    //        return;
    //    }

    //    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    //}

    //internal class DescriptionConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Description) || t == typeof(Description?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "Image":
    //                return Description.Image;

    //            case "Promise.then":
    //                return Description.PromiseThen;

    //            case "load":
    //                return Description.Load;
    //        }
    //        throw new Exception("Cannot unmarshal type Description");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Description)untypedValue;
    //        switch (value)
    //        {
    //            case Description.Image:
    //                serializer.Serialize(writer, "Image");
    //                return;

    //            case Description.PromiseThen:
    //                serializer.Serialize(writer, "Promise.then");
    //                return;

    //            case Description.Load:
    //                serializer.Serialize(writer, "load");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type Description");
    //    }

    //    public static readonly DescriptionConverter Singleton = new DescriptionConverter();
    //}

    //internal class TypeEnumConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "other":
    //                return TypeEnum.Other;

    //            case "parser":
    //                return TypeEnum.Parser;

    //            case "preflight":
    //                return TypeEnum.Preflight;

    //            case "script":
    //                return TypeEnum.Script;
    //        }
    //        throw new Exception("Cannot unmarshal type TypeEnum");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (TypeEnum)untypedValue;
    //        switch (value)
    //        {
    //            case TypeEnum.Other:
    //                serializer.Serialize(writer, "other");
    //                return;

    //            case TypeEnum.Parser:
    //                serializer.Serialize(writer, "parser");
    //                return;

    //            case TypeEnum.Preflight:
    //                serializer.Serialize(writer, "preflight");
    //                return;

    //            case TypeEnum.Script:
    //                serializer.Serialize(writer, "script");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type TypeEnum");
    //    }

    //    public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    //}

    //internal class PriorityConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Priority) || t == typeof(Priority?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "High":
    //                return Priority.High;

    //            case "Low":
    //                return Priority.Low;

    //            case "VeryHigh":
    //                return Priority.VeryHigh;
    //        }
    //        throw new Exception("Cannot unmarshal type Priority");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Priority)untypedValue;
    //        switch (value)
    //        {
    //            case Priority.High:
    //                serializer.Serialize(writer, "High");
    //                return;

    //            case Priority.Low:
    //                serializer.Serialize(writer, "Low");
    //                return;

    //            case Priority.VeryHigh:
    //                serializer.Serialize(writer, "VeryHigh");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type Priority");
    //    }

    //    public static readonly PriorityConverter Singleton = new PriorityConverter();
    //}

    //internal class PagerefConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Pageref) || t == typeof(Pageref?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        if (value == "page_1")
    //        {
    //            return Pageref.Page1;
    //        }
    //        throw new Exception("Cannot unmarshal type Pageref");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Pageref)untypedValue;
    //        if (value == Pageref.Page1)
    //        {
    //            serializer.Serialize(writer, "page_1");
    //            return;
    //        }
    //        throw new Exception("Cannot marshal type Pageref");
    //    }

    //    public static readonly PagerefConverter Singleton = new PagerefConverter();
    //}

    //internal class HttpVersionConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(HttpVersion) || t == typeof(HttpVersion?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "h3":
    //                return HttpVersion.H3;

    //            case "http/1.1":
    //                return HttpVersion.Http11;

    //            case "http/2.0":
    //                return HttpVersion.Http20;
    //        }
    //        throw new Exception("Cannot unmarshal type HttpVersion");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (HttpVersion)untypedValue;
    //        switch (value)
    //        {
    //            case HttpVersion.H3:
    //                serializer.Serialize(writer, "h3");
    //                return;

    //            case HttpVersion.Http11:
    //                serializer.Serialize(writer, "http/1.1");
    //                return;

    //            case HttpVersion.Http20:
    //                serializer.Serialize(writer, "http/2.0");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type HttpVersion");
    //    }

    //    public static readonly HttpVersionConverter Singleton = new HttpVersionConverter();
    //}

    //internal class MethodConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Method) || t == typeof(Method?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "GET":
    //                return Method.Get;

    //            case "OPTIONS":
    //                return Method.Options;

    //            case "PUT":
    //                return Method.Put;
    //        }
    //        throw new Exception("Cannot unmarshal type Method");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Method)untypedValue;
    //        switch (value)
    //        {
    //            case Method.Get:
    //                serializer.Serialize(writer, "GET");
    //                return;

    //            case Method.Options:
    //                serializer.Serialize(writer, "OPTIONS");
    //                return;

    //            case Method.Put:
    //                serializer.Serialize(writer, "PUT");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type Method");
    //    }

    //    public static readonly MethodConverter Singleton = new MethodConverter();
    //}

    //internal class StatusTextConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(StatusText) || t == typeof(StatusText?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "":
    //                return StatusText.Empty;

    //            case "Internal Redirect":
    //                return StatusText.InternalRedirect;
    //        }
    //        throw new Exception("Cannot unmarshal type StatusText");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (StatusText)untypedValue;
    //        switch (value)
    //        {
    //            case StatusText.Empty:
    //                serializer.Serialize(writer, "");
    //                return;

    //            case StatusText.InternalRedirect:
    //                serializer.Serialize(writer, "Internal Redirect");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type StatusText");
    //    }

    //    public static readonly StatusTextConverter Singleton = new StatusTextConverter();
    //}

    //internal class ServerIpAddressConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(ServerIpAddress) || t == typeof(ServerIpAddress?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "":
    //                return ServerIpAddress.Empty;

    //            case "172.67.220.149":
    //                return ServerIpAddress.The17267220149;
    //        }
    //        throw new Exception("Cannot unmarshal type ServerIpAddress");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (ServerIpAddress)untypedValue;
    //        switch (value)
    //        {
    //            case ServerIpAddress.Empty:
    //                serializer.Serialize(writer, "");
    //                return;

    //            case ServerIpAddress.The17267220149:
    //                serializer.Serialize(writer, "172.67.220.149");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type ServerIpAddress");
    //    }

    //    public static readonly ServerIpAddressConverter Singleton = new ServerIpAddressConverter();
    //}
}