using rsc_converter.Classes;
using rsc_converter.Classes.Generators;
using rsc_converter.Classes.Utils;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace rsc_converter
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
            session.ClassGenerators.Add(new ArmorsGenerator());
            session.ClassGenerators.Add(new CapacitorsGenerator());
            session.ClassGenerators.Add(new FoodsGenerator());
            session.ClassGenerators.Add(new MenusGenerator());
            session.ClassGenerators.Add(new MachinesGenerator());
            session.ClassGenerators.Add(new GeneratorsGenerator());
            session.ClassGenerators.Add(new SolarGeneratorsGenerator());
            session.ClassGenerators.Add(new PluginMainGenerator());
            session.FileGenerators.Add(new PluginYamlGenerator());
            session.Build();
        }
    }
}
