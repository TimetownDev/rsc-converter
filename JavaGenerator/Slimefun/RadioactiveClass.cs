namespace JavaGenerator.Slimefun;

public static class RadioactiveClass
{
    public static ClassDefinition Class { get; }
    static RadioactiveClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Radioactive")
        {
            NeedGenerate = false
        };
    }
}
