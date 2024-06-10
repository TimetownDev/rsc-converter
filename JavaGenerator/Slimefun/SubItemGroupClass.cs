namespace rsc_converter.JavaGenerator.Slimefun;

public static class SubItemGroupClass
{
    public static ClassDefinition Class { get; }
    static SubItemGroupClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api.items.groups", "SubItemGroup")
        {
            NeedGenerate = false,
            Super = ItemGroupClass.Class
        };
    }
}
