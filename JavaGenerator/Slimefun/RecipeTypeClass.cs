namespace JavaGenerator.Slimefun;

public static class RecipeTypeClass
{
    public static ClassDefinition Class { get; }
    static RecipeTypeClass()
    {
        Class = new ClassDefinition("io.github.thebusybiscuit.slimefun4.api.recipes", "RecipeType")
        {
            NeedGenerate = false
        };
    }
}
