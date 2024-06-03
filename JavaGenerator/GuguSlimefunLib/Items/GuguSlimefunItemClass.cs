using JavaGenerator.GuguSlimefunLib.Script;
using JavaGenerator.Slimefun;
using JavaGenerator.System;

namespace JavaGenerator.GuguSlimefunLib.Items;

public static class GuguSlimefunItemClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition SetRainbowType { get; }
    public static MethodDefinition SetAntiWither { get; }
    public static MethodDefinition SetPiglinTradeChance { get; }
    public static MethodDefinition SetDropFrom { get; }
    public static MethodDefinition SetDropChance { get; }
    public static MethodDefinition SetDropAmount { get; }
    public static MethodDefinition SetVanilla { get; }
    public static MethodDefinition SetHidden { get; }
    public static MethodDefinition SetEval { get; }
    public static MethodDefinition Register { get; }
    static GuguSlimefunItemClass()
    {
        Class = new ClassDefinition("me.ddggdd135.guguslimefunlib.items", "GuguSlimefunItem")
        {
            NeedGenerate = false
        };
        SetRainbowType = new("setRainbowType")
        {
            ParameterTypes = [RainbowTypeClass.Class]
        };
        Class.Methods.Add(SetRainbowType);
        SetAntiWither = new("setAntiWither")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetAntiWither);
        SetPiglinTradeChance = new("setPiglinTradeChance")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetPiglinTradeChance);
        SetDropFrom = new("setDropFrom")
        {
            ParameterTypes = [SystemClass.StringClass]
        };
        Class.Methods.Add(SetDropFrom);
        SetDropChance = new("setDropChance")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetDropChance);
        SetDropAmount = new("setDropAmount")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetDropAmount);
        SetVanilla = new("setVanilla")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetVanilla);
        SetHidden = new("setHidden")
        {
            ParameterTypes = [SystemClass.BooleanClass]
        };
        Class.Methods.Add(SetHidden);
        SetEval = new("setEval")
        {
            ParameterTypes = [ScriptEvalClass.Class]
        };
        Class.Methods.Add(SetEval);
        Register = new("register")
        {
            ParameterTypes = [SlimefunAddonClass.Class]
        };
        Class.Methods.Add(Register);
    }
}
