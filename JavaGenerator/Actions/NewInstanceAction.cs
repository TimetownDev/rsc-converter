using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator.Actions;

public class NewInstanceAction : IAction, IValue
{
    public ClassDefinition ClassDefinition { get; set; }
    public IList<IValue> Parameters { get; set; } = [];
    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append("new");
        sb.Append(' ');
        sb.Append(ClassDefinition.OnImport(classDefinition));
        sb.Append('(');

        int x = 0;
        foreach (IValue parameter in Parameters)
        {
            string next = parameter.BuildContent(classDefinition);
            if (next.Trim() != string.Empty)
            {
                if (x != 0)
                    sb.Append(", ");
                sb.Append(next);
            }
            x++;
        }
        sb.Append(')');

        return sb.ToString();
    }
    public NewInstanceAction(ClassDefinition classDefinition, params IValue[] parameters)
    {
        ClassDefinition = classDefinition;
        Parameters = parameters;
    }
}
