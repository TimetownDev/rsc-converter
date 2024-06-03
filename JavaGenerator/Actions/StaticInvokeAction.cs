using JavaGenerator.Exceptions;
using JavaGenerator.Interfaces;
using System.Text;

namespace JavaGenerator.Actions;

public class StaticInvokeAction : IAction, IValue
{
    public ClassDefinition ClassDefinition { get; set; }
    public MethodDefinition MethodDefinition { get; set; }
    public IList<IValue> Parameters { get; set; } = [];

    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(ClassDefinition.OnImport(classDefinition));
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
    public StaticInvokeAction(ClassDefinition classDefinition, MethodDefinition methodDefinition, params IValue[] parameters)
    {
        if (!classDefinition.Methods.Contains(methodDefinition))
            throw new NoSuchMethodException(methodDefinition);
        if (!methodDefinition.IsStatic)
            throw new InvalidOperationException("方法不是静态");
        ClassDefinition = classDefinition;
        MethodDefinition = methodDefinition;
        Parameters = parameters;
    }
}
