using rsc_converter.JavaGenerator.Attributes;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Utils;
using System.Text;

namespace rsc_converter.JavaGenerator;

public class CtorMethodDefinition
{
    public AccessAttribute Access { get; set; } = AccessAttribute.Public;
    public IList<IClassDefinition> ParameterTypes { get; set; } = [];
    public ActionBlock Block { get; set; } = new();
    public virtual string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(Access.ToString().ToLower());
        sb.Append(' ');
        sb.Append(classDefinition.Name);
        sb.Append('(');
        //我们遵循param + x的命名方式 x从0开始
        int x = 0;
        foreach (IClassDefinition parameter in ParameterTypes)
        {
            sb.Append(parameter.OnImport(classDefinition));
            sb.Append(' ');
            sb.Append($"param{x}");
            if (x + 1 != ParameterTypes.Count)
                sb.Append(", ");
            x++;
        }
        sb.Append(") {\n");
        sb.Append(IndentationUtils.Indente(Block.BuildContent(classDefinition)));
        sb.Append('}');

        return sb.ToString();
    }

    public CtorMethodDefinition(IList<IClassDefinition> parameterTypes)
    {
        ParameterTypes = parameterTypes;
    }

    public CtorMethodDefinition(params IClassDefinition[] parameterTypes)
    {
        ParameterTypes = parameterTypes.ToList();
    }
}
