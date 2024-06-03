namespace JavaGenerator.Slimefun;

public static class ProtectiveArmorClass
{
    public static ClassDefinition Class { get; }
    static ProtectiveArmorClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "ProtectiveArmor")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
