namespace rsc_converter.JavaGenerator.Bukkit;

public static class PotionEffectClass
{
    public static ClassDefinition Class { get; }
    static PotionEffectClass()
    {
        Class = new("org.bukkit.potion", "PotionEffect")
        {
            NeedGenerate = false
        };
    }
}
