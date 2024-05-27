using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator.Values;

public class ThisValue : IValue
{
    public string BuildContent(ClassDefinition classDefinition)
    {
        return "this";
    }
}
