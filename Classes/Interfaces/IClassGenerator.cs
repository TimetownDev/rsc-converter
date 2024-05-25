using rscconventer.JavaGenerator;

namespace rscconventer.Classes.Interfaces;

public interface IClassGenerator
{
    IList<ClassDefinition>? OnGenerate(BuildSession session);
}
