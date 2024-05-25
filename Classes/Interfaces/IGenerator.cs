using rscconventer.JavaGenerator;

namespace rscconventer.Classes.Interfaces;

public interface IGenerator
{
    ClassDefinition? OnGenerate(BuildSession session);
}
