namespace rsc_converter.JavaGenerator.Slimefun;

public static class CapacitorClass
{
    public static ClassDefinition Class { get; }
    static CapacitorClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.implementation.items.electric", "Capacitor")
        {
            NeedGenerate = false
        };
    }
}
