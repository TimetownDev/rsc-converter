using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Utils;
using System.Text;

namespace rscconventer.JavaGenerator;

public class ClassDefinition
{
    private ClassDefinition? super;

    public bool NeedGenerate { get; set; } = true;
    public IList<ClassDefinition> Imported { get; set; } = [];
    public IList<ClassAttribute> Attributes { get; set; } = [];
    public AccessAttribute AccessAttribute { get; set; } = AccessAttribute.Public;
    public string Name { get; set; } = string.Empty;
    public Namespace Namespace { get; set; }
    public ClassDefinition? Super
    {
        get
        {
            return super;
        }
        set
        {
            if (value == null)
            {
                super = value;
                return;
            }
            if (value == this || value.Attributes.Contains(ClassAttribute.Abstract)) return;
            super = value;
        }
    }
    public IList<ClassDefinition> Interfaces { get; set; } = [];
    public IList<MethodDefinition> Methods { get; set; } = [];

    public ClassDefinition(string @namespace, string name)
    {
        Namespace = new Namespace(@namespace);
        Name = name;
    }
    public ClassDefinition(Namespace @namespace, string name)
    {
        Namespace = @namespace;
        Name = name;
    }

    public override string ToString()
    {
        StringBuilder importBuilder = new();
        StringBuilder sb = new();
        importBuilder.Append("package ");
        importBuilder.Append(Namespace);
        importBuilder.Append(';');
        importBuilder.Append('\n');
        importBuilder.Append('\n');
        // 先构建内容
        sb.Append(AccessAttribute.ToString().ToLower());
        sb.Append(' ');
        if (Attributes.Contains(ClassAttribute.Interface))
        {
            sb.Append("interface");
        }
        else if (Attributes.Contains(ClassAttribute.Abstract))
        {
            sb.Append("abstract");
            sb.Append(' ');
            sb.Append("class");
        }
        else
        {
            sb.Append("class");
        }
        sb.Append(' ');
        sb.Append(Name);
        sb.Append(' ');
        if (Super != null)
        {
            sb.Append("extends");
            sb.Append(' ');
            ImportUtils.Import(Imported, Super);
            sb.Append(ImportUtils.GetUsing(Imported, Super));
            sb.Append(' ');
        }
        if (Interfaces.Count > 0)
        {
            sb.Append("implements");
            sb.Append(' ');
            sb.Append(string.Join(", ", Interfaces.Select(x =>
            {
                ImportUtils.Import(Imported, x);
                return ImportUtils.GetUsing(Imported, x);
            })));
            sb.Append(' ');
        }
        sb.Append('{');
        sb.Append('\n');
        foreach (MethodDefinition methodDefinition in Methods)
        {
            sb.Append(methodDefinition.ToString(this));
            sb.Append('\n');
        }
        sb.Append('\n');
        sb.Append('}');

        foreach (ClassDefinition classDefinition in Imported)
        {
            importBuilder.Append("import ");
            importBuilder.Append(classDefinition.Namespace);
            importBuilder.Append('.');
            importBuilder.Append(classDefinition.Name);
            importBuilder.Append(';');
            importBuilder.Append('\n');
        }
        importBuilder.Append('\n');

        return importBuilder.ToString() + sb.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is not ClassDefinition classDefinition) return false;
        return Name == classDefinition.Name && Namespace == classDefinition.Namespace;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() ^ Namespace.GetHashCode();
    }

    public static bool operator ==(ClassDefinition? a, ClassDefinition? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(ClassDefinition? a, ClassDefinition? b)
    {
        return !(a == b);
    }
}
