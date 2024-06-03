using JavaGenerator.Bukkit;
using JavaGenerator.System;

namespace JavaGenerator.GuguSlimefunLib.Utils;

public static class PotionUtilsClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition Parse { get; }
    public static MethodDefinition ParseAll { get; }
    static PotionUtilsClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.utils", "PotionUtils")
        {
            NeedGenerate = false
        };
        Parse = new("parse")
        {
            ReturnType = PotionEffectClass.Class,
            ParameterTypes = [SystemClass.StringClass],
            IsStatic = true
        };
        Class.Methods.Add(Parse);
        ParseAll = new("parseAll")
        {
            ReturnType = new ArrayClassDefinition(PotionEffectClass.Class),
            ParameterTypes = [new ParamsClassDefinition(SystemClass.StringClass)],
            IsStatic = true
        };
        Class.Methods.Add(ParseAll);
    }
}
