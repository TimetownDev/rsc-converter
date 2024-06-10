using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.Slimefun;

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
            ReturnType = Class,
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(ValueOf);
    }
}
