using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Utils;
using System.Text;

namespace rscconventer.JavaGenerator;

public class MethodDefinition
{
    public AccessAttribute AccessAttribute { get; set; } = AccessAttribute.Public;
    public string Name { get; set; } = string.Empty;
    public ClassDefinition? ReturnType { get; set; }
    public IList<ClassDefinition> ParameterTypes { get; set; } = [];
    public ActionBlock Block { get; set; } = new ActionBlock();
    public string ToString(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(AccessAttribute.ToString().ToLower());
        sb.Append(' ');
        if (ReturnType == null)
        {
            sb.Append("void");
        }
        else
        {
            ImportUtils.Import(classDefinition.Imported, ReturnType);
            sb.Append(ImportUtils.GetUsing(classDefinition.Imported, ReturnType));
        }
        sb.Append(' ');
        sb.Append(Name);
        sb.Append('(');
        //我们遵循param + x的命名方式 x从0开始
        int x = 0;
        foreach (ClassDefinition parameter in ParameterTypes)
        {
            ImportUtils.Import(classDefinition.Imported, parameter);
            sb.Append(ImportUtils.GetUsing(classDefinition.Imported, parameter));
            sb.Append(' ');
            sb.Append($"param{x}");
            if (x + 1 != ParameterTypes.Count)
                sb.Append(", ");
            x++;
        }
        sb.Append(") {\n");
        sb.Append(Block.ToString(classDefinition));
        sb.Append("}");

        return sb.ToString();
    }

    public MethodDefinition(string name)
    {
        Name = name;
    }
}
