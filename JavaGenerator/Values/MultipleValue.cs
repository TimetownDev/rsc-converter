using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator.Values;

public class MultipleValue : IValue
{
    public IList<IValue> Values { get; set; } = [];
    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();

        int x = 0;
        foreach (IValue value in Values)
        {
            if (value == null)
                sb.Append("null");
            else
                sb.Append(value.BuildContent(classDefinition));
            if (x + 1 != Values.Count)
                sb.Append(", ");
            x++;
        }

        return sb.ToString();
    }
    public MultipleValue() { }
    public MultipleValue(IList<IValue> values)
    {
        Values = values;
    }
    public MultipleValue(IList<string> values)
    {
        foreach (string value in values)
        {
            Values.Add(new StringValue(value));
        }
    }
}
