using System;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal class PluginConfiguration
    {
        private PluginConfiguration() { }

        private PluginConfiguration(string name, string usage, bool isFilter, int[] positional, object named)
        {
            IsFilter = isFilter;
            Name = name;
            Usage = usage;
            Named = named;
            Positional = positional;
        }

        public static PluginConfiguration Create() => new PluginConfiguration();

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("usage")]
        public string Usage { get; }

        [JsonPropertyName("is_filter")]
        public bool IsFilter { get; } = true;

        [JsonPropertyName("positional")]
        public int[] Positional { get; } = Array.Empty<int>();

        [JsonPropertyName("named")]
        public object Named { get; } = new { };

        public PluginConfiguration WithName(string name) => new PluginConfiguration(
            name,
            this.Usage,
            this.IsFilter,
            this.Positional,
            this.Named
        );

        public PluginConfiguration WithUsage(string usage) => new PluginConfiguration(
            this.Name,
            usage,
            this.IsFilter,
            this.Positional,
            this.Named
        );

        public PluginConfiguration WithIsFilter(bool isFilter) => new PluginConfiguration(
            this.Name,
            this.Usage,
            isFilter,
            this.Positional,
            this.Named
        );
    }
}