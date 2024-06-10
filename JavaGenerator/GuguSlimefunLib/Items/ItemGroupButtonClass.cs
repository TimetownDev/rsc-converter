using rsc_converter.JavaGenerator.Slimefun;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class ItemGroupButtonClass
{
    public static ClassDefinition Class { get; }
    static ItemGroupButtonClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.items", "ItemGroupButton")
        {
            NeedGenerate = false,
            Super = ItemGroupClass.Class
        };
    }
}
