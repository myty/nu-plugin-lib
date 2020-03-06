using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal class Signature
    {
        private Signature()
        {
        }

        private Signature(
            string                                name, string usage, bool isFilter, int[] positional,
            int[]                                 restPositional,
            IReadOnlyDictionary<string, object[]> named,
            object                                yields, object input
        )
        {
            IsFilter       = isFilter;
            Name           = name;
            Description    = usage;
            Named          = named;
            Positional     = positional;
            RestPositional = restPositional;
            Yields         = yields;
            Input          = input;
        }

        public static Signature Create() => new Signature();

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("usage")]
        public string Description { get; }

        [JsonPropertyName("positional")]
        public int[] Positional { get; } = Array.Empty<int>();

        [JsonPropertyName("rest_positional")]
        public int[] RestPositional { get; }

        [JsonPropertyName("named")]
        public IReadOnlyDictionary<string, object[]> Named { get; } =
            new Dictionary<string, object[]>
                {{"help", new object[] {new NamedTypeSwitch('h'), "Display this help message"}}};

        [JsonPropertyName("yields")]
        public object Yields { get; }

        [JsonPropertyName("input")]
        public object Input { get; }

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

        public Signature AddNamedSwitch(string name, string description, char? flag = null)
        {
            if (!flag.HasValue)
            {
                flag = name.FirstOrDefault();
            }

            var namedTypeSwitch = new NamedTypeSwitch(flag.Value);
            var named           = this.Named.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            named.Add(name, new object[] {namedTypeSwitch, description});

            return new Signature(
                this.Name,
                this.Description,
                this.IsFilter,
                this.Positional,
                this.RestPositional,
                named,
                this.Yields,
                this.Input
            );
        }

        public Signature AddNamedOptional(string name, SyntaxShape syntaxShape, string description, char? flag = null)
        {
            var flagValue         = new string(new char[] {flag ?? name.FirstOrDefault()});
            var namedTypeOptional = new NamedTypeOptional(new string[] {flagValue, syntaxShape.Shape});
            var named             = this.Named.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            named.Add(name, new object[] {namedTypeOptional, description});

            return new Signature(
                this.Name,
                this.Description,
                this.IsFilter,
                this.Positional,
                this.RestPositional,
                named,
                this.Yields,
                this.Input
            );
        }
    }
}
