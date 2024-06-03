namespace JavaGenerator.GuguSlimefunLib.Items;

public static class GuguSlimefunArmorPieceClass
{
    public static ClassDefinition Class { get; }
    static GuguSlimefunArmorPieceClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "GuguSlimefunArmorPiece")
        {
            NeedGenerate = false
        };
    }
}
