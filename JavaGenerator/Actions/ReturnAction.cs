using rsc_converter.JavaGenerator.Interfaces;
using System.Text;

namespace rsc_converter.JavaGenerator.Actions;

public class ReturnAction : IAction
{
    public IValue? Value { get; set; }
    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();
        sb.Append("return");
        sb.Append(' ');
        if (Value == null)
            sb.Append("null");
        else
            sb.Append(Value.BuildContent(classDefinition));

        return sb.ToString();
    }
    public ReturnAction() { }
    public ReturnAction(IValue? value)
    {
        Value = value;
    }
}
