using JavaGenerator.Interfaces;

namespace JavaGenerator.Values;

public class BoolValue : IValue
{
    public bool Value { get; set; }

    public BoolValue()
    {
        Value = false;
    }
    public BoolValue(bool value)
    {
        Value = value;
    }

    public string BuildContent(ClassDefinition classDefinition)
    {
        return Value.ToString().ToLower();
    }
}
