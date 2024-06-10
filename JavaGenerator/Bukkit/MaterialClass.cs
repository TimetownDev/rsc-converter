namespace rsc_converter.JavaGenerator.Bukkit;

public static class MaterialClass
{
    public static ClassDefinition Class { get; }
    static MaterialClass()
    {
        Class = new ClassDefinition("org.bukkit", "Material")
        {
            NeedGenerate = false
        };
    }
}
