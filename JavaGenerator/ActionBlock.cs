using rscconventer.JavaGenerator.Interfaces;
using System.Text;

namespace rscconventer.JavaGenerator;

public class ActionBlock
{
    public IList<IAction> Actions { get; set; } = [];
    public string ToString(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        foreach (IAction action in Actions)
        {
            sb.Append(action.BuildContent(classDefinition));
            sb.Append(";\n");
        }

        return sb.ToString();
    }
    public ActionBlock(params IAction[] actions)
    {
        Actions = actions.ToList();
    }
}
