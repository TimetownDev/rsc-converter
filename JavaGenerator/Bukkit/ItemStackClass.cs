namespace rsc_converter.JavaGenerator.Bukkit;

public static class ItemStackClass
{
    public static ClassDefinition Class { get; }
    static ItemStackClass()
    {
        Class = new ClassDefinition("org.bukkit.inventory", "ItemStack")
        {
            NeedGenerate = false
        };
    }
}
