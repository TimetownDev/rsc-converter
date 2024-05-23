using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator;

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
        sb.Append(Value);
        sb.Append('\"');
        return sb.ToString();
    }
}
