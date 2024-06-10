using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator.Actions;

public class ObjectInvokeAction : IAction, IValue
{
    public IValue Value { get; set; }
    public MethodDefinition MethodDefinition { get; set; }
    public IList<IValue> Parameters { get; set; } = [];

    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(Value.BuildContent(classDefinition));
        sb.Append('.');
        sb.Append(MethodDefinition.Name);
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
    public ObjectInvokeAction(IValue value, MethodDefinition methodDefinition, params IValue[] parameters)
    {
        if (methodDefinition.IsStatic)
            throw new InvalidOperationException("方法不是非静态");
        Value = value;
        MethodDefinition = methodDefinition;
        Parameters = parameters;
    }
}
