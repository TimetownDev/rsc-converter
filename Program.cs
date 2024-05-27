using rscconventer.Classes;
using rscconventer.Classes.Generators;
using rscconventer.Classes.Utils;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.System;
using rscconventer.JavaGenerator.Values;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace rscconventer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ClassDefinition sfItem = new("io.github.thebusybiscuit.slimefun4.api.items", "SlimefunItem");
            ClassDefinition classDefinition = new("me.ddggdd135", "testClass")
            {
                Super = sfItem,
                IsAbstract = true
            };
            MethodDefinition methodDefinition = new("testMethod");
            methodDefinition.ParameterTypes.Add(SystemClass.StringClass);
            methodDefinition.Block.Actions.Add(BukkitClass.Class.Invoke(BukkitClass.GetConsoleSender).Invoke(ConsoleCommandSenderClass.SendMessage, new ParameterValue(0)));
            classDefinition.Methods.Add(methodDefinition);
            classDefinition.FieldList.Add(new(SystemClass.StringClass, "testStringField", new StringValue("\n")));

            Console.WriteLine(classDefinition.BuildContent());
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("");
            }

            YamlStream stream = [];
            stream.Load(new StringReader(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "info.yml"))));
            YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
            string name = yaml.GetString("id", "");
            if (name.Trim() == string.Empty) throw new ArgumentException("id不能为空");
            BuildSession session = new(new(Environment.CurrentDirectory));
            session.Name = name;
            session.ClassGenerators.Add(new ItemGroupGenerator());
            session.ClassGenerators.Add(new RecipeTypeGenerator());
            session.ClassGenerators.Add(new PluginMainGenerator());
            session.FileGenerators.Add(new PluginYamlGenerator());
            session.Build();
        }
    }
}
