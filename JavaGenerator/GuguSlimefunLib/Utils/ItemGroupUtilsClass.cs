using JavaGenerator.Bukkit;
using JavaGenerator.GuguSlimefunLib.Items;
using JavaGenerator.Slimefun;
using JavaGenerator.System;

namespace JavaGenerator.GuguSlimefunLib.Utils;

public static class ItemGroupUtilsClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition FindItemGroup { get; }
    public static MethodDefinition CreateSubItemGroup { get; }
    public static MethodDefinition CreateButton { get; }
    static ItemGroupUtilsClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.utils", "ItemGroupUtils")
        {
            NeedGenerate = false
        };
        FindItemGroup = new MethodDefinition("findItemGroup")
        {
            IsStatic = true,
            ReturnType = ItemGroupClass.Class,
            ParameterTypes = [NamespacedKeyClass.Class]
        };
        Class.Methods.Add(FindItemGroup);
        CreateSubItemGroup = new MethodDefinition("createSubItemGroup")
        {
            IsStatic = true,
            ReturnType = SubItemGroupClass.Class,
            ParameterTypes = [NamespacedKeyClass.Class, NamespacedKeyClass.Class, ItemStackClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(CreateSubItemGroup);
        CreateButton = new MethodDefinition("createButton")
        {
            IsStatic = true,
            ReturnType = ItemGroupButtonClass.Class,
            ParameterTypes = [NamespacedKeyClass.Class, NamespacedKeyClass.Class, ItemStackClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(CreateButton);
    }
}
