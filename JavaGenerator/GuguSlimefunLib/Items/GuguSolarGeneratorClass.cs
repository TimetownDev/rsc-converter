using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class GuguSolarGeneratorClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition SetLightLevel { get; }
    static GuguSolarGeneratorClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.api.abstracts", "GuguSolarGenerator")
        {
            NeedGenerate = false
        };
        SetLightLevel = new("setLightLevel")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetLightLevel);
    }
}
