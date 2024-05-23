using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

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
