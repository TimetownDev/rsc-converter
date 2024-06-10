using rsc_converter.JavaGenerator.Interfaces;

namespace rsc_converter.JavaGenerator;

public class ParameterValue : IValue
{
    public int id { get; set; } = 0;

    public string BuildContent(ClassDefinition classDefinition)
    {
        return $"param{id}";
    }
    public ParameterValue(int id)
    {
        this.id = id;
    }
}
