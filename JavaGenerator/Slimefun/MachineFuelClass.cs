namespace rsc_converter.JavaGenerator.Slimefun;

public static class MachineFuelClass
{
    public static ClassDefinition Class { get; }
    static MachineFuelClass()
    {
        Class = new("me.mrCookieSlime.Slimefun.Objects.SlimefunItem.abstractItems", "MachineFuel")
        {
            NeedGenerate = false
        };
    }
}
