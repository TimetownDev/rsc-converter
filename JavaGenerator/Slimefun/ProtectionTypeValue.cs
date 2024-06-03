using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator.Slimefun;

public class ProtectionTypeValue : IValue
{
    public string ProtectionName { get; set; }

    public string BuildContent(ClassDefinition classDefinition)
    {
        return ProtectionTypeClass.Class.OnImport(classDefinition) + "." + ProtectionName.ToUpper();
    }
    public ProtectionTypeValue(string protectionName)
    {
        ProtectionName = protectionName;
    }
}
