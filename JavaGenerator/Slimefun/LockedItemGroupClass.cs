namespace JavaGenerator.Slimefun;

public static class LockedItemGroupClass
{
    public static ClassDefinition Class { get; }
    static LockedItemGroupClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.api.items.groups", "LockedItemGroup")
        {
            NeedGenerate = false,
            Super = ItemGroupClass.Class
        };
    }
}
