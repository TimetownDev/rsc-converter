namespace rsc_converter.JavaGenerator.Slimefun;

public static class BlockMenuPresetClass
{
    public static ClassDefinition Class { get; }
    static BlockMenuPresetClass()
    {
        Class = new("me.mrCookieSlime.Slimefun.api.inventory", "BlockMenuPreset")
        {
            NeedGenerate = false
        };
    }
}
