using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Utils;
using System.Text;

namespace rscconventer.JavaGenerator;

public class StaticInvokeAction : IAction, IValue
{
    public ClassDefinition ClassDefinition { get; set; }
    public MethodDefinition MethodDefinition { get; set; }
    public IList<IValue> Parameters { get; set; } = [];

    public string ToString(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        ImportUtils.Import(classDefinition.Imported, ClassDefinition);
        sb.Append(ImportUtils.GetUsing(classDefinition.Imported, ClassDefinition));
        sb.Append('.');
        sb.Append(MethodDefinition.Name);
        sb.Append('(');

        int x = 0;
        foreach (IValue parameter in Parameters)
        {
            sb.Append(parameter.ToString(classDefinition));
            if (x + 1 !=  Parameters.Count)
                sb.Append(", ");
            x++;
        }
        sb.Append(')');

        return sb.ToString();
    }
    public StaticInvokeAction(ClassDefinition classDefinition, MethodDefinition methodDefinition, params IValue[] parameters)
    {
        ClassDefinition = classDefinition;
        MethodDefinition = methodDefinition;
        Parameters = parameters;
    }
}
