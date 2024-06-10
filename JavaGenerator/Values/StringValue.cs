using rsc_converter.Classes.Utils;
using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator.Values;

public class StringValue : IValue
{
    public string Value { get; set; } = string.Empty;
    public StringValue() { }
    public StringValue(string value)
    {
        Value = value;
    }

    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append('\"');
        sb.Append(Value.Escape());
        sb.Append('\"');
        return sb.ToString();
    }
}
