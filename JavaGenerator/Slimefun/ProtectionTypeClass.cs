using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.System;

namespace rscconventer.JavaGenerator.Slimefun;

public static class ProtectionTypeClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition ValueOf { get; }
    static ProtectionTypeClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "ProtectionType")
        {
            NeedGenerate = false
        };
        ValueOf = new("valueOf")
        {
            IsStatic = true,
            ReturnType = ProtectionTypeClass.Class,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(ValueOf);
    }
}
