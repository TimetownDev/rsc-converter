namespace JavaGenerator.Slimefun;

public static class SoulboundClass
{
    public static ClassDefinition Class { get; }
    static SoulboundClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "Soulbound")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
