using JavaGenerator.Interfaces;
using System.Text;

namespace JavaGenerator.Values;

public class ArrayValue : IValue
{
    public IClassDefinition Type { get; set; }
    public IList<IValue> Values { get; set; }
    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append("new");
        sb.Append(' ');
        sb.Append(Type.OnImport(classDefinition));
        sb.Append("[]");
        sb.Append(' ');
        sb.Append('{');
        sb.Append(' ');
        int x = 0;
        foreach (IValue value in Values)
        {
            string next = value == null ? "null" : value.BuildContent(classDefinition);
            if (next.Trim() != string.Empty)
            {
                if (x != 0)
                    sb.Append(", ");
                sb.Append(next);
            }
            x++;
        }
        sb.Append(' ');
        sb.Append('}');

        return sb.ToString();
    }
    public ArrayValue(IClassDefinition type, IList<IValue> values)
    {
        Type = type;
        Values = values;
    }
}
