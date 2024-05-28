namespace rscconventer.JavaGenerator.Interfaces;

public interface IClassDefinition
{
    public string Name { get; }
    public string OnImport(ClassDefinition classDefinition);
}
