using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    public class JsonRpcParams
    {
        [JsonPropertyName("value")]
        public ParamValue Value { get; set; }

        [JsonPropertyName("tag")]
        public ParamTag Tag { get; set; }

        public class ParamValue
        {
            public IDictionary<string, object> Primitive { get; set; }
        }

        public class ParamTag
        {
            [JsonPropertyName("anchor")]
            public string Anchor { get; set; }

            [JsonPropertyName("span")]
            public ParamTagSpan Span { get; set; }

            public class ParamTagSpan
            {
                [JsonPropertyName("start")]
                public int Start { get; set; }

                [JsonPropertyName("end")]
                public int End { get; set; }
            }
        }
    }
}
