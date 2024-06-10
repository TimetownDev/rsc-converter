using rsc_converter.JavaGenerator.Interfaces;

namespace rsc_converter.JavaGenerator.Bukkit;

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
