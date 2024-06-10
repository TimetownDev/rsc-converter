namespace rsc_converter.JavaGenerator.Slimefun;

public static class RechargeableClass
{
    public static ClassDefinition Class { get; }
    static RechargeableClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Rechargeable")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
