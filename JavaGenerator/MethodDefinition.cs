using rsc_converter.JavaGenerator.Attributes;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Utils;
using System.Text;

namespace rsc_converter.JavaGenerator;

public class MethodDefinition : IStaticable, IAccessable
{
    public AccessAttribute Access { get; set; } = AccessAttribute.Public;
    public string Name { get; set; } = string.Empty;
    public IClassDefinition? ReturnType { get; set; }
    public IList<IClassDefinition> ParameterTypes { get; set; } = [];
    public bool IsStatic { get; set; } = false;
    public ActionBlock Block { get; set; } = new ActionBlock();
    public virtual string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(Access.ToString().ToLower());
        sb.Append(' ');
        if (IsStatic)
        {
            sb.Append("static");
            sb.Append(' ');
        }
        if (ReturnType == null)
        {
            sb.Append("void");
        }
        else
        {
            sb.Append(ReturnType.OnImport(classDefinition));
        }
        sb.Append(' ');
        sb.Append(Name);
        sb.Append('(');
        //我们遵循param + x的命名方式 x从0开始
        int x = 0;
        foreach (ClassDefinition parameter in ParameterTypes)
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

    public MethodDefinition(string name)
    {
        Name = name;
    }
}
