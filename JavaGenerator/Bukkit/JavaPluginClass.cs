namespace rsc_converter.JavaGenerator.Bukkit;

public static class JavaPluginClass
{
    public static ClassDefinition Class { get; }
    static JavaPluginClass()
    {
        Class = new("org.bukkit.plugin.java", "JavaPlugin")
        {
            NeedGenerate = false
        };
    }
}
