using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

public class RawValue : IValue
{
    public ImportList ImportList = [];
    public string ValueText { get; set; } = string.Empty;
    public string BuildContent(ClassDefinition classDefinition)
    {
        classDefinition.ImportList.Merge(ImportList);
        return ValueText;
    }
    public RawValue(string valueText)
    {
        ValueText = valueText;
    }
}
