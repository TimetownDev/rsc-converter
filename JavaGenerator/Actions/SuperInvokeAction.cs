using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator.Actions;

public class SuperInvokeAction : IAction
{
    public IList<IValue> Parameters { get; set; } = [];

    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append("super");
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
    public SuperInvokeAction(IList<IValue> parameters)
    {
        Parameters = parameters;
    }
    public SuperInvokeAction(params IValue[] parameters)
    {
        Parameters = parameters.ToList();
    }
}
