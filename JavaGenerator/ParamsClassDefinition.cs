using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

public class ParamsClassDefinition : IClassDefinition
{
    public IClassDefinition Class { get; set; }
    public string Name => $"{Class.Name}...";
    public ParamsClassDefinition(IClassDefinition classDefinition)
    {
        Class = classDefinition;
    }
}
