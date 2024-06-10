namespace rsc_converter.JavaGenerator.Bukkit;

public static class WorldEnvironmentClass
{
    public static ClassDefinition Class { get; }
    static WorldEnvironmentClass()
    {
        Class = new("org.bukkit.World", "Environment")
        {
            NeedGenerate = false
        };
    }
}
