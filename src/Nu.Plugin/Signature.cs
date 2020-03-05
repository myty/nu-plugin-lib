using System;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal class Signature
    {
        private Signature()
        {
        }

        private Signature(
            string name,   string usage, bool isFilter, int[] positional, int[] restPositional, object named,
            object yields, object input
        )
        {
            IsFilter       = isFilter;
            Name           = name;
            Description    = usage;
            Named          = named;
            Positional     = positional;
            RestPositional = restPositional;
        }

        public static Signature Create() => new Signature();

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("usage")]
        public string Description { get; }

        [JsonPropertyName("positional")]
        public int[] Positional { get; } = Array.Empty<int>();

        [JsonPropertyName("rest_positional")]
        public int[] RestPositional { get; } = null;

        [JsonPropertyName("named")]
        public object Named { get; } = new { };

        [JsonPropertyName("yields")]
        public object Yields { get; } = null;

        [JsonPropertyName("input")]
        public object Input { get; } = null;

        [JsonPropertyName("is_filter")]
        public bool IsFilter { get; }

        public Signature WithName(string name) => new Signature(
            name,
            this.Description,
            this.IsFilter,
            this.Positional,
            this.RestPositional,
            this.Named,
            this.Yields,
            this.Input
        );

        public Signature WithDescription(string description) => new Signature(
            this.Name,
            description,
            this.IsFilter,
            this.Positional,
            this.RestPositional,
            this.Named,
            this.Yields,
            this.Input
        );

        public Signature WithIsFilter(bool isFilter) => new Signature(
            this.Name,
            this.Description,
            isFilter,
            this.Positional,
            this.RestPositional,
            this.Named,
            this.Yields,
            this.Input
        );
    }
}
