namespace rsc_converter.JavaGenerator.GuguSlimefunLib.Items;

public static class GuguMachineBlockClass
{
    public static ClassDefinition Class { get; }
    static GuguMachineBlockClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.api.abstracts", "GuguMachineBlock")
        {
            NeedGenerate = false
        };
    }
}
