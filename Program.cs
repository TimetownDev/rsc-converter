using rscconventer.Classes;
using rscconventer.Classes.Generators;
using rscconventer.Classes.Utils;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace rscconventer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            YamlStream stream = [];
            stream.Load(new StringReader(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "info.yml"))));
            YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
            string name = yaml.GetString("id", "");
            if (name.Trim() == string.Empty) throw new ArgumentException("id不能为空");
            BuildSession session = new(new(Environment.CurrentDirectory))
            {
                Name = name
            };

            session.ClassGenerators.Add(new ItemsGenerator());
            session.ClassGenerators.Add(new ItemGroupGenerator());
            session.ClassGenerators.Add(new RecipeTypeGenerator());
            session.ClassGenerators.Add(new GEOResourceGenerator());
            session.ClassGenerators.Add(new MobDropsGenerator());
            session.ClassGenerators.Add(new SimpleItemsGenerator());
            session.ClassGenerators.Add(new FoodsGenerator());
            session.ClassGenerators.Add(new PluginMainGenerator());
            session.FileGenerators.Add(new PluginYamlGenerator());
            session.Build();
        }
    }
}
