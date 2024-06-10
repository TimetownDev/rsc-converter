namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class AdvancedNestedItemGroupClass
{
    public static ClassDefinition Class { get; }
    static AdvancedNestedItemGroupClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.items", "AdvancedNestedItemGroup")
        {
            NeedGenerate = false
        };
    }
}
