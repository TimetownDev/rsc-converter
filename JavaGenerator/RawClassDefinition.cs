using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

public class RawClassDefinition : IClassDefinition
{
    public string Name { get; set; }

    public string Namespace => "";

    public string FullName => Name;
    public bool NeedImport => false;

    public RawClassDefinition(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is not RawClassDefinition returnType) return false;
        return Name == returnType.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public string OnImport(ClassDefinition classDefinition)
    {
        return Name;
    }

    public static bool operator ==(RawClassDefinition? a, RawClassDefinition? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(RawClassDefinition? a, RawClassDefinition? b)
    {
        return !(a == b);
    }
}
