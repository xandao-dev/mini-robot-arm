﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace RobotArmAPP.Classes
{
    public partial class Moves
    {

        [JsonProperty("repeatTimes")]
        public long RepeatTimes { get; set; }

        [JsonProperty("moves")]
        public List<Move> Movements { get; set; }

        [JsonProperty("rpt")]
        public long Rpt { get; set; }

        [JsonProperty("mov")]
        public List<List<long>> Mov { get; set; }
    }

    public partial class Move
    {
        [JsonProperty("axis1")]
        public string Axis1 { get; set; }

        [JsonProperty("axis2")]
        public string Axis2 { get; set; }

        [JsonProperty("axis3")]
        public string Axis3 { get; set; }

        [JsonProperty("axis4")]
        public string Axis4 { get; set; }

        [JsonProperty("garra")]
        public string Garra { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }

        [JsonProperty("delay")]
        public string Delay { get; set; }
    }

    public partial class Moves
    {
        public static Moves FromJson(string json) => JsonConvert.DeserializeObject<Moves>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Moves self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal } }
        };
    }
}
