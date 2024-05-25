namespace rscconventer.JavaGenerator.Slimefun;

public static class SlimefunAddonClass
{
    public static ClassDefinition Class { get; }
    static SlimefunAddonClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api", "SlimefunAddon")
        {
            NeedGenerate = false
        };
    }
}
