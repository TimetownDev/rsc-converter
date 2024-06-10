using rsc_converter.JavaGenerator;

namespace rsc_converter.Classes.Interfaces;

public interface IClassGenerator
{
    IList<ClassDefinition>? OnGenerate(BuildSession session);
}
