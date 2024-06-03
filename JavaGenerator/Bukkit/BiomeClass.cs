using JavaGenerator.System;

namespace JavaGenerator.Bukkit;

public static class BiomeClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition ValueOf { get; }
    static BiomeClass()
    {
        Class = new("org.bukkit.block", "Biome")
        {
            NeedGenerate = false
        };
        ValueOf = new("valueOf")
        {
            IsStatic = true,
            ReturnType = Class,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(ValueOf);
    }
}
