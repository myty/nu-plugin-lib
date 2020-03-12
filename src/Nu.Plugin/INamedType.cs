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
        public NamedTypeMandatory(string[] mandatory) => Mandatory = mandatory;

        [JsonPropertyName("Mandatory")]
        public string[] Mandatory { get; }
    }

    public class NamedTypeOptional : INamedType
    {
        public NamedTypeOptional(string[] optional) => Optional = optional;

        [JsonPropertyName("Optional")]
        public string[] Optional { get; }
    }
}
