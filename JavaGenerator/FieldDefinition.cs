using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator;

public class FieldDefinition
{
    public AccessAttribute Access { get; set; } = AccessAttribute.Public;
    public ClassDefinition Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public IValue? DefaultValue { get; set; }
    public bool IsStatic { get; set; } = false;
    public FieldDefinition(ClassDefinition type, string name, IValue? defaultValue)
    {
        Type = type;
        Name = name;
        DefaultValue = defaultValue;
    }
    public FieldDefinition(ClassDefinition type, string name)
    {
        Type = type;
        Name = name;
    }

    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append(Access.ToString().ToLower());
        sb.Append(' ');
        if (IsStatic)
        {
            sb.Append("static");
            sb.Append(' ');
        }
        classDefinition.ImportList.Add(Type);
        sb.Append(classDefinition.ImportList.GetUsing(Type));
        sb.Append(' ');
        sb.Append(Name);
        if (DefaultValue != null)
        {
            sb.Append(" = ");
            sb.Append(DefaultValue.BuildContent(classDefinition));
        }

        return sb.ToString();
    }
}
