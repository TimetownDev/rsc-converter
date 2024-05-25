using rscconventer.Classes.Interfaces;
using rscconventer.JavaGenerator;

namespace rscconventer.Classes;

public class BuildSession
{
    public IList<ClassDefinition> ClassDefinitions { get; set; } = [];
    public IList<IGenerator> Generators { get; set; } = [];
    public DirectoryInfo Directory { get; set; } = new(Environment.CurrentDirectory);
    public string Name { get; set; } = string.Empty;
    public ClassDefinition? PluginMainClass { get; private set; }
    public void Build()
    {
        foreach (IGenerator generator in Generators)
        {
            ClassDefinition? generated = generator.OnGenerate(this);
            if (generator != null) ClassDefinitions.Add(generated);
        }
    }
    public BuildSession(string name, DirectoryInfo directory)
    {
        Name = name;
        Directory = directory;
    }
}
