using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class SupplyInfoClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition SetMaxDeviation { get; }
    public static MethodDefinition SetObtainFromGEOMiner { get; }
    public static MethodDefinition SetDefaultNormalSupply { get; }
    public static MethodDefinition SetDefaultNetherSupply { get; }
    public static MethodDefinition SetDefaultTheEndSupply { get; }
    public static MethodDefinition AddNormalSupply { get; }
    public static MethodDefinition AddNetherSupply { get; }
    public static MethodDefinition AddTheEndSupply { get; }
    public static MethodDefinition GetDefaultSupply { get; }
    static SupplyInfoClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "SupplyInfo")
        {
            NeedGenerate = false
        };
        SetMaxDeviation = new("setMaxDeviation")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetMaxDeviation);
        SetObtainFromGEOMiner = new("setObtainFromGEOMiner")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetObtainFromGEOMiner);
        SetDefaultNormalSupply = new("setDefaultNormalSupply")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetDefaultNormalSupply);
        SetDefaultNetherSupply = new("setDefaultNetherSupply")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetDefaultNetherSupply);
        SetDefaultTheEndSupply = new("setDefaultTheEndSupply")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetDefaultTheEndSupply);
        AddNormalSupply = new("addNormalSupply")
        {
            ParameterTypes = [BiomeClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(AddNormalSupply);
        AddNetherSupply = new("addNetherSupply")
        {
            ParameterTypes = [BiomeClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(AddNetherSupply);
        AddTheEndSupply = new("addTheEndSupply")
        {
            ParameterTypes = [BiomeClass.Class, SystemClass.IntegerClass]
        };
        Class.Methods.Add(AddTheEndSupply);
        GetDefaultSupply = new("getDefaultSupply")
        {
            ReturnType = new RawClassDefinition("int"),
            ParameterTypes = [WorldEnvironmentClass.Class, BiomeClass.Class]
        };
        Class.Methods.Add(GetDefaultSupply);
    }
}
