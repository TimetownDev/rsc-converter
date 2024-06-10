using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Utils;

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
