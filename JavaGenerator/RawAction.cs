using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

public class RawAction : IAction
{
    public string ActionText { get; set; } = string.Empty;
    public ImportList ImportList = [];

    public string BuildContent(ClassDefinition classDefinition)
    {
        classDefinition.ImportList.Merge(ImportList);
        return ActionText;
    }
}
