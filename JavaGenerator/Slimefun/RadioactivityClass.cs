namespace JavaGenerator.Slimefun;

public static class RadioactivityClass
{
    public static ClassDefinition Class { get; }
    static RadioactivityClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Radioactivity")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
