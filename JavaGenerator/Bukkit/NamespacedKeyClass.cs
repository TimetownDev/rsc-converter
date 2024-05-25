using rscconventer.JavaGenerator.System;

namespace rscconventer.JavaGenerator.Bukkit;

public static class NamespacedKeyClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition FromString { get; }
    static NamespacedKeyClass()
    {
        Class = new ClassDefinition("org.bukkit", "NamespacedKey")
        {
            NeedGenerate = false
        };
        FromString = new MethodDefinition("fromString")
        {
            IsStatic = true,
            ReturnType = NamespacedKeyClass.Class,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(FromString);
    }
}
