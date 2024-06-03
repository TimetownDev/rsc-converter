namespace JavaGenerator.Slimefun;

public static class SeasonalItemGroupClass
{
    public static ClassDefinition Class { get; }
    static SeasonalItemGroupClass()
    {
        Class = new ClassDefinition("io.github.thebusybiscuit.slimefun4.api.items.groups", "SeasonalItemGroup")
        {
            NeedGenerate = false,
            Super = ItemGroupClass.Class
        };
    }
}
