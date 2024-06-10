using rsc_converter.JavaGenerator.Interfaces;

namespace rsc_converter.JavaGenerator.Actions;

public class RawAction : IAction
{
    public string ActionText { get; set; } = string.Empty;
    public ImportList ImportList = [];

    public string BuildContent(ClassDefinition classDefinition)
    {
        classDefinition.ImportList.Merge(ImportList);
        return ActionText;
    }
    public RawAction(string actionText)
    {
        ActionText = actionText;
    }
}
