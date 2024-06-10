namespace rsc_converter.JavaGenerator;

public class Namespace
{
    public string Name { get; set; } = string.Empty;
    public Namespace(string name)
    {
        Name = name;
    }
    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is not Namespace @namespace) return false;
        return Name == @namespace.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static bool operator ==(Namespace? a, Namespace? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(Namespace? a, Namespace? b)
    {
        return !(a == b);
    }
}
