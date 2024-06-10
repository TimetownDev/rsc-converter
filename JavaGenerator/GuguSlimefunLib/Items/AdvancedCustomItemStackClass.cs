using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class AdvancedCustomItemStackClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition FromHashCode { get; }
    public static MethodDefinition FromBase64 { get; }
    public static MethodDefinition FromURL { get; }
    public static MethodDefinition DoGlow { get; }
    public static MethodDefinition SetCustomModelData { get; }
    public static MethodDefinition AsQuantity { get; }
    public static MethodDefinition Parse { get; }
    public static MethodDefinition FromSlimefunItem { get; }
    public static MethodDefinition FromLore { get; }
    public static MethodDefinition MakeChanceLore { get; }
    static AdvancedCustomItemStackClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "AdvancedCustomItemStack")
        {
            NeedGenerate = false,
            Super = ItemStackClass.Class
        };
        FromHashCode = new("fromHashCode")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass],
        };
        Class.Methods.Add(FromHashCode);
        FromBase64 = new("fromBase64")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass]
        };
        Class.Methods.Add(FromBase64);
        FromURL = new("fromURL")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass]
        };
        Class.Methods.Add(FromURL);
        DoGlow = new("doGlow")
        {
            ReturnType = Class
        };
        Class.Methods.Add(DoGlow);
        SetCustomModelData = new("setCustomModelData")
        {
            ReturnType = Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetCustomModelData);
        SetCustomModelData = new("setCustomModelData")
        {
            ReturnType = Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetCustomModelData);
        AsQuantity = new("asQuantity")
        {
            ReturnType = Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(AsQuantity);
        Parse = new("parse")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(Parse);
        FromSlimefunItem = new("fromSlimefunItem")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, new ParamsClassDefinition(SystemClass.StringClass)]
        };
        Class.Methods.Add(FromSlimefunItem);
        FromLore = new("fromLore")
        {
            ReturnType = Class,
            IsStatic = true,
            ParameterTypes = [ItemStackClass.Class, new ParamsClassDefinition(SystemClass.StringClass)]
        };
        Class.Methods.Add(FromLore);
        MakeChanceLore = new("makeChanceLore")
        {
            ReturnType = SystemClass.StringClass,
            IsStatic = true,
            ParameterTypes = [EntityTypeClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(MakeChanceLore);
    }
}
