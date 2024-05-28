using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.System;

namespace rscconventer.JavaGenerator.GuguSlimefunLib.Items;

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
    static AdvancedCustomItemStackClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "AdvancedCustomItemStack")
        {
            NeedGenerate = false,
            Super = ItemStackClass.Class
        };
        FromHashCode = new("fromHashCode")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass],
        };
        Class.Methods.Add(FromHashCode);
        FromBase64 = new("fromBase64")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass]
        };
        Class.Methods.Add(FromBase64);
        FromURL = new("fromURL")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, SystemClass.StringClass]
        };
        Class.Methods.Add(FromURL);
        DoGlow = new("doGlow")
        {
            ReturnType = AdvancedCustomItemStackClass.Class
        };
        Class.Methods.Add(DoGlow);
        SetCustomModelData = new("setCustomModelData")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetCustomModelData);
        SetCustomModelData = new("setCustomModelData")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetCustomModelData);
        AsQuantity = new("asQuantity")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(AsQuantity);
        Parse = new("parse")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(Parse);
        FromSlimefunItem = new("fromSlimefunItem")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass, SystemClass.StringClass, new ParamsClassDefinition(SystemClass.StringClass)]
        };
        Class.Methods.Add(FromSlimefunItem);
        FromLore = new("fromLore")
        {
            ReturnType = AdvancedCustomItemStackClass.Class,
            IsStatic = true,
            ParameterTypes = [ItemStackClass.Class, new ParamsClassDefinition(SystemClass.StringClass)]
        };
        Class.Methods.Add(FromLore);
    }
}
