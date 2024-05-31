namespace rscconventer.JavaGenerator.Slimefun;

public static class RandomMobDropClass
{
    public static ClassDefinition Class { get; }
    static RandomMobDropClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "RandomMobDrop")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
