using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;
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
            for (int i = 0; i < 4; i ++)
            {
                Console.WriteLine("");
            }

            YamlStream stream = new();
            stream.Load(new StringReader("name: \"&6&l隐藏物品\"\r\nmaterial: BARRIER"));
            YamlNode root = stream.Documents[0].RootNode;
            IValue item = ItemReader.ReadItem(root, new(Environment.CurrentDirectory));
            Console.WriteLine(item.BuildContent(classDefinition));
        }
    }
}
