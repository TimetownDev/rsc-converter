using JavaGenerator.Interfaces;

namespace JavaGenerator.Values;

public class RawValue : IValue
{
    public ImportList ImportList { get; set; } = [];
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
