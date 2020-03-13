using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal class Signature
    {
        private Signature() { }

        private Signature(
            string name,
            string usage,
            bool isFilter,
            object[][] positional,
            string[] restPositional,
            IReadOnlyDictionary<string, object[]> named,
            object yields, object input
        )
        {
            IsFilter = isFilter;
            Name = name;
            Description = usage;
            Named = named;
            Positional = positional;
            RestPositional = restPositional;
            Yields = yields;
            Input = input;
        }

        public static Signature Create() => new Signature();

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("usage")]
        public string Description { get; }

        [JsonPropertyName("positional")]
        public object[][] Positional { get; } = Array.Empty<object[]>();

        [JsonPropertyName("rest_positional")]
        public string[] RestPositional { get; } = null;

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
            Description,
            IsFilter,
            Positional,
            RestPositional,
            Named,
            Yields,
            Input
        );

        public Signature WithDescription(string description) => new Signature(
            Name,
            description,
            IsFilter,
            Positional,
            RestPositional,
            Named,
            Yields,
            Input
        );

        public Signature WithIsFilter(bool isFilter) => new Signature(
            Name,
            Description,
            isFilter,
            Positional,
            RestPositional,
            Named,
            Yields,
            Input
        );

        public Signature AddRequiredPositional(SyntaxShape syntaxShape, string name, string description)
        {
            var positionalArgument = new object[]
            {
                new MandatoryPostionalType(name, syntaxShape),
                description
            };

            var newPositionalArguments = new object[Positional.Length + 1][];
            if (Positional.Length > 0)
            {
                Positional.CopyTo(newPositionalArguments, 0);
            }

            newPositionalArguments[Positional.Length] = positionalArgument;

            return new Signature(
                Name,
                Description,
                IsFilter,
                newPositionalArguments,
                RestPositional,
                Named,
                Yields,
                Input
            );
        }

        public Signature AddOptionalPositional(SyntaxShape syntaxShape, string name, string description)
        {
            var positionalArgument = new object[]
            {
                new OptionalPostionalType(name, syntaxShape),
                description
            };

            var newPositionalArguments = new object[Positional.Length + 1][];
            if (Positional.Length > 0)
            {
                Positional.CopyTo(newPositionalArguments, 0);
            }

            newPositionalArguments[Positional.Length] = positionalArgument;

            return new Signature(
                Name,
                Description,
                IsFilter,
                newPositionalArguments,
                RestPositional,
                Named,
                Yields,
                Input
            );
        }

        public Signature AddRestPositionalArguments(string description, SyntaxShape syntaxShape = null)
        {
            var newRestPositional = new string[]
            {
                (syntaxShape ?? SyntaxShape.Any).Shape,
                description
            };

            return new Signature(
                Name,
                Description,
                IsFilter,
                Positional,
                newRestPositional,
                Named,
                Yields,
                Input
            );
        }

        public Signature AddSwitch(string name, string description, char? flag = null)
        {
            if (!flag.HasValue)
            {
                flag = name.FirstOrDefault();
            }

            var namedTypeSwitch = new NamedTypeSwitch(flag.Value);
            var named = Named.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            named.Add(name, new object[] { namedTypeSwitch, description });

            return new Signature(
                Name,
                Description,
                IsFilter,
                Positional,
                RestPositional,
                named,
                Yields,
                Input
            );
        }

        public Signature AddOptionalNamed(SyntaxShape syntaxShape, string name, string description, char? flag = null)
        {
            var flagValue = new string(new char[] { flag ?? name.FirstOrDefault() });
            var namedTypeOptional = new NamedTypeOptional(flagValue, syntaxShape);
            var named = Named.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            named.Add(name, new object[] { namedTypeOptional, description });

            return new Signature(
                Name,
                Description,
                IsFilter,
                Positional,
                RestPositional,
                named,
                Yields,
                Input
            );
        }

        public Signature AddRequiredNamed(SyntaxShape syntaxShape, string name, string description, char? flag = null)
        {
            var flagValue = new string(new char[] { flag ?? name.FirstOrDefault() });
            var namedTypeMandatory = new NamedTypeMandatory(flagValue, syntaxShape);
            var named = Named.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            named.Add(name, new object[] { namedTypeMandatory, description });

            return new Signature(
                Name,
                Description,
                IsFilter,
                Positional,
                RestPositional,
                named,
                Yields,
                Input
            );
        }
    }
}
