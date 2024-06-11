using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Script;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class MachineMenuClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition SetEval { get; }
    public static MethodDefinition AddItemEx { get; }
    public static MethodDefinition SetProgressBar { get; }
    public static MethodDefinition SetProgressSlot { get; }
    static MachineMenuClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.api", "MachineMenu")
        {
            NeedGenerate = false
        };
        SetEval = new("setEval")
        {
            ParameterTypes = [ScriptEvalClass.Class]
        };
        Class.Methods.Add(SetEval);
        AddItemEx = new("addItem")
        {
            ParameterTypes = [ItemStackClass.Class, new ParamsClassDefinition(SystemClass.IntegerClass)],
            ReturnType = MachineMenuClass.Class
        };
        Class.Methods.Add(AddItemEx);
        SetProgressBar = new("setProgressBar")
        {
            ParameterTypes = [ItemStackClass.Class]
        };
        Class.Methods.Add(SetProgressBar);
        SetProgressSlot = new("setProgressSlot")
        {
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(SetProgressSlot);
    }
}
