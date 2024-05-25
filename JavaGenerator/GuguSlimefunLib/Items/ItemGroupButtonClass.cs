using rscconventer.JavaGenerator.Slimefun;

namespace rscconventer.JavaGenerator.GuguSlimefunLib.Items;

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
