using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Exceptions;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Utils;
using System.Text;

namespace rscconventer.JavaGenerator;

public class ClassDefinition : IAccessable
{
    private ClassDefinition? super;
    private bool isInterface = false;
    private bool isAbstract = false;

    public bool NeedGenerate { get; set; } = true;
    public ImportList ImportList { get; set; } = [];
    public FieldList FieldList { get; set; } = [];
    public bool IsInterface
    {
        get
        {
            return isInterface;
        }
        set
        {
            if (value)
                isAbstract = false;
            isInterface = value;
        }
    }
    public bool IsAbstract
    {
        get
        {
            return isAbstract;
        }
        set
        {
            if (value)
                isInterface = false;
            isAbstract = value;
        }
    }
    public AccessAttribute Access { get; set; } = AccessAttribute.Public;
    public string Name { get; set; } = string.Empty;
    public Namespace Namespace { get; set; }
    public string FullName 
    {
        get
        {
            return Namespace + "." + Name;
        }
    }
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
            if (value == this) return;
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

    public string BuildContent()
    {
        StringBuilder importBuilder = new();
        StringBuilder sb = new();
        importBuilder.Append("package ");
        importBuilder.Append(Namespace);
        importBuilder.Append(';');
        importBuilder.Append('\n');
        importBuilder.Append('\n');
        // 先构建内容
        sb.Append(Access.ToString().ToLower());
        sb.Append(' ');
        if (IsInterface)
        {
            sb.Append("interface");
        }
        else if (IsAbstract)
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
            ImportList.Import(Super);
            sb.Append(ImportList.GetUsing(Super));
            sb.Append(' ');
        }
        if (Interfaces.Count > 0)
        {
            sb.Append("implements");
            sb.Append(' ');
            sb.Append(string.Join(", ", Interfaces.Select(x =>
            {
                ImportList.Import(x);
                return ImportList.GetUsing(x);
            })));
            sb.Append(' ');
        }
        sb.Append('{');
        sb.Append('\n');
        if (FieldList.Count != 0)
        {
            sb.Append(IndentationUtils.Indente(FieldList.BuildContent(this)));
            sb.Append('\n');
            sb.Append('\n');
        }
        foreach (MethodDefinition methodDefinition in Methods)
        {
            sb.Append(IndentationUtils.Indente(methodDefinition.BuildContent(this)));
            sb.Append('\n');
        }
        sb.Append('}');

        importBuilder.Append(ImportList.BuildContent());
        importBuilder.Append('\n');
        importBuilder.Append('\n');

        return importBuilder.ToString() + sb.ToString();
    }

    public MethodDefinition? FindMethod(string name, bool throwException = false)
    {
        foreach (MethodDefinition method in Methods)
        {
            if (method.Name == name) return method;
        }

        if (throwException)
            throw new NoSuchMethodException(name);

        return null;
    }

    public StaticInvokeAction Invoke(string name, params IValue[] parameters)
    {
        MethodDefinition? method = FindMethod(name);
        if (method == null || !method.IsStatic)
            throw new NoSuchMethodException(name);

        return new StaticInvokeAction(this, method, parameters);
    }

    public StaticInvokeAction Invoke(MethodDefinition method, params IValue[] parameters)
    {
        if (!Methods.Contains(method))
            throw new InvalidOperationException("此方法不归本类所有");
        if (!method.IsStatic)
            throw new InvalidOperationException("不能直接调用非静态方法");
        return new StaticInvokeAction(this, method, parameters);
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
