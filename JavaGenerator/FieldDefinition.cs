using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator;

public class FieldDefinition : IStaticable, IAccessable
{
    public AccessAttribute Access { get; set; } = AccessAttribute.Public;
    public IClassDefinition Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public IValue? DefaultValue { get; set; }
    public bool IsStatic { get; set; } = false;
    public FieldDefinition(IClassDefinition type, string name, IValue? defaultValue)
    {
        Type = type;
        Name = name;
        DefaultValue = defaultValue;
    }
    public FieldDefinition(IClassDefinition type, string name)
    {
        Type = type;
        Name = name;
    }

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
        if (Type is ClassDefinition type)
        {
            classDefinition.ImportList.Import(type);
            sb.Append(classDefinition.ImportList.GetUsing(type));
        }
        else
            sb.Append(Type.Name);

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
