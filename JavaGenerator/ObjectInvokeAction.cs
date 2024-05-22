using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator;

public class ObjectInvokeAction : IAction, IValue
{
    public IValue Value { get; set; }
    public MethodDefinition MethodDefinition { get; set; }
    public IList<IValue> Parameters { get; set; } = [];

    public string ToString(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(Value.ToString(classDefinition));
        sb.Append('.');
        sb.Append(MethodDefinition.Name);
        sb.Append('(');

        int x = 0;
        foreach (IValue parameter in Parameters)
        {
            sb.Append(parameter.ToString(classDefinition));
            if (x + 1 != Parameters.Count)
                sb.Append(", ");
            x++;
        }
        sb.Append(')');

        return sb.ToString();
    }
    public ObjectInvokeAction(IValue value, MethodDefinition methodDefinition, params IValue[] parameters)
    {
        Value = value;
        MethodDefinition = methodDefinition;
        Parameters = parameters;
    }
}
