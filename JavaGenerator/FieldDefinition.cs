using rsc_converter.JavaGenerator.Attributes;
using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator;

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
            sb.Append(type.OnImport(classDefinition));
        }
        else
            sb.Append(Type.OnImport(classDefinition));

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
