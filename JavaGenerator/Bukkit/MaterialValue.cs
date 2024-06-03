using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator.Bukkit;

public class MaterialValue : IValue
{
    public static MaterialValue Air { get; } = new("AIR");
    public string MaterialName { get; set; } = "STONE";

    public string BuildContent(ClassDefinition classDefinition)
    {
        return MaterialClass.Class.OnImport(classDefinition) + "." + MaterialName.ToUpper();
    }
    public MaterialValue(string materialName)
    {
        MaterialName = materialName;
    }
}
