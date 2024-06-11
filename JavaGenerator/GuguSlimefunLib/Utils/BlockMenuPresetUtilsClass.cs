using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Utils;

public static class BlockMenuPresetUtilsClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition FindBlockMenuPreset { get; }
    static BlockMenuPresetUtilsClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.utils", "BlockMenuPresetUtils")
        {
            NeedGenerate = false
        };
        FindBlockMenuPreset = new("findBlockMenuPreset")
        {
            IsStatic = true,
            ParameterTypes = [SystemClass.StringClass],
            ReturnType = BlockMenuPresetClass.Class
        };
        Class.Methods.Add(FindBlockMenuPreset);
    }
}
