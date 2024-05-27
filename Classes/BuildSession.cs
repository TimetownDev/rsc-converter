using rscconventer.Classes.Interfaces;
using rscconventer.JavaGenerator;

namespace rscconventer.Classes;

public class BuildSession
{
    public IList<ClassDefinition> ClassDefinitions { get; set; } = [];
    public IList<IClassGenerator> ClassGenerators { get; set; } = [];
    public IList<FileData> Files { get; set; } = [];
    public IList<IFileGenerator> FileGenerators { get; set; } = [];
    public DirectoryInfo Directory { get; set; } = new(Environment.CurrentDirectory);
    public DirectoryInfo SaveDirectory { get; set; } = new DirectoryInfo(Environment.CurrentDirectory).CreateSubdirectory("target");
    public string Name { get; set; } = string.Empty;
    public void Build()
    {
        foreach (IClassGenerator generator in ClassGenerators)
        {
            IList<ClassDefinition>? generateds = generator.OnGenerate(this);
            if (generateds != null)
            {
                foreach (ClassDefinition generated in generateds)
                {
                    ClassDefinitions.Add(generated);
                }
            }
        }

        foreach (IFileGenerator generator in FileGenerators)
        {
            IList<FileData>? generateds = generator.OnGenerate(this);
            if (generateds != null)
            {
                foreach (FileData generated in generateds)
                {
                    Files.Add(generated);
                }
            }
        }

        // 保存所有文件
        foreach (ClassDefinition classDefinition in ClassDefinitions)
        {
            string path = Path.Combine(SaveDirectory.FullName, "src/main/java", classDefinition.Namespace.Name.Replace(".", "/"), classDefinition.Name + ".java");
            FileInfo fileInfo = new(path);
            fileInfo.Directory!.Create();
            File.WriteAllText(path, classDefinition.BuildContent());
        }

        foreach (FileData file in Files)
        {
            string path = Path.Combine(SaveDirectory.FullName, file.Path, file.Name);
            FileInfo fileInfo = new(path);
            fileInfo.Directory!.Create();
            File.WriteAllBytes(path, file.Data);
        }
    }
    public ClassDefinition? GetClassDefinition(string name)
    {
        return ClassDefinitions.FirstOrDefault(x => x.Name == name);
    }
    public BuildSession(DirectoryInfo directory)
    {
        Directory = directory;
    }
}
