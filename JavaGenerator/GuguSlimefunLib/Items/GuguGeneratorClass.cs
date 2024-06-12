using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class GuguGeneratorClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition SetCapacity { get; }
    public static MethodDefinition SetEnergyProduction { get; }
    public static MethodDefinition RegisterFuel { get; }
    static GuguGeneratorClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.api.abstracts", "GuguGenerator")
        {
            NeedGenerate = false,
            Super = GuguSlimefunItemClass.Class
        };
        SetCapacity = new("setCapacity")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetCapacity);
        SetEnergyProduction = new("setEnergyProduction")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetEnergyProduction);
        RegisterFuel = new("registerFuel")
        {
            ParameterTypes = [MachineFuelClass.Class]
        };
        Class.Methods.Add(RegisterFuel);
    }
}
