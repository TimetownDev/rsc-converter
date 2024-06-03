namespace JavaGenerator.GuguSlimefunLib.Items;

public static class RainbowTypeClass
{
    public static ClassDefinition Class { get; }
    static RainbowTypeClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.items", "RainbowType")
        {
            NeedGenerate = false
        };
    }
}
