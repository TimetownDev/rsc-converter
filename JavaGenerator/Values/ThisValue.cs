using JavaGenerator.Interfaces;

namespace JavaGenerator.Values;

public class ThisValue : IValue
{
    public string BuildContent(ClassDefinition classDefinition)
    {
        return "this";
    }
}
