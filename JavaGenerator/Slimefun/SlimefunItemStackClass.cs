namespace rsc_converter.JavaGenerator.Slimefun;

public static class SlimefunItemStackClass
{
    public static ClassDefinition Class { get; }
    static SlimefunItemStackClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api.items", "SlimefunItemStack")
        {
            NeedGenerate = false
        };
    }
}
