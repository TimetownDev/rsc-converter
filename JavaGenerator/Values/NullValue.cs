using rsc_converter.JavaGenerator.Interfaces;

namespace rsc_converter.JavaGenerator.Values;

public class NullValue : IValue
{
    public string BuildContent(ClassDefinition classDefinition)
    {
        return "null";
    }
}
