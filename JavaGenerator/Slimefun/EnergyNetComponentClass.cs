namespace rsc_converter.JavaGenerator.Slimefun;

public static class EnergyNetComponentClass
{
    public static ClassDefinition Class { get; }
    static EnergyNetComponentClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "EnergyNetComponent")
        {
            NeedGenerate = false
        };
    }
}
