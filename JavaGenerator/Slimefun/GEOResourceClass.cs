namespace JavaGenerator.Slimefun;

public static class GEOResourceClass
{
    public static ClassDefinition Class { get; }
    static GEOResourceClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api.geo", "GEOResource")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
