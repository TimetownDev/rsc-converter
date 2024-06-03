namespace JavaGenerator.Slimefun;

public static class NotPlaceableClass
{
    public static ClassDefinition Class { get; }
    static NotPlaceableClass()
    {
        Class = new("io.github.thebusybiscuit.slimefun4.core.attributes", "NotPlaceable")
        {
            NeedGenerate = false,
            IsInterface = true
        };
    }
}
