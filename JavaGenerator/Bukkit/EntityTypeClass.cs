namespace rsc_converter.JavaGenerator.Bukkit;

public static class EntityTypeClass
{
    public static ClassDefinition Class { get; }
    static EntityTypeClass()
    {
        Class = new ClassDefinition("org.bukkit.entity", "EntityType")
        {
            NeedGenerate = false
        };
    }
}
