using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    public interface INamedType { }

    public class NamedTypeSwitch : INamedType
    {
        public NamedTypeSwitch(char switchChar) => Switch = switchChar;

        [JsonPropertyName("Switch")]
        public char Switch { get; }
    }

    public class NamedTypeMandatory : INamedType
    {
        public NamedTypeMandatory(string flagValue) : this(flagValue, SyntaxShape.Any) { }
        public NamedTypeMandatory(string flagValue, SyntaxShape syntax) => Mandatory = new string[] { flagValue, syntax.Shape };

        [JsonPropertyName("Mandatory")]
        public string[] Mandatory { get; }
    }

    public class NamedTypeOptional : INamedType
    {
        public NamedTypeOptional(string flagValue) : this(flagValue, SyntaxShape.Any) { }
        public NamedTypeOptional(string flagValue, SyntaxShape syntax) => Optional = new string[] { flagValue, syntax.Shape };

        [JsonPropertyName("Optional")]
        public string[] Optional { get; }
    }
}
