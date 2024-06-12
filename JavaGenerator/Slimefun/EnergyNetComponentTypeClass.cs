namespace rsc_converter.JavaGenerator.Slimefun;

public static class EnergyNetComponentTypeClass
{
    public static ClassDefinition Class { get; }
    static EnergyNetComponentTypeClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.networks.energy", "EnergyNetComponentType")
        {
            NeedGenerate = false
        };
    }
}
