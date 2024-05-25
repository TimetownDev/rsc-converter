namespace rscconventer.JavaGenerator.Slimefun;

public static class ItemGroupClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition Register { get; }
    static ItemGroupClass()
    {
        Class = new ClassDefinition("io.github.thebusybiscuit.slimefun4.api.items", "ItemGroup")
        {
            NeedGenerate = false,
        };
        Register = new("register")
        {
            ParameterTypes = [SlimefunAddonClass.Class]
        };
        Class.Methods.Add(Register);
    }
}
