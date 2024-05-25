using rscconventer.Classes;
using rscconventer.Classes.Generators;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.System;
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

            YamlStream stream = [];
            stream.Load(new StringReader("rsc_example_normal_group:\r\n  item:\r\n    name: \"&a示例普通物品组\"\r\n    material: GRASS_BLOCK\r\n\r\nrsc_example_parent_group:\r\n  type: nested\r\n  item:\r\n    name: \"&a示例父物品组\"\r\n    material: OAK_PLANKS\r\n\r\nrsc_example_sub_group:\r\n  type: sub\r\n  parent: \"rsc_example_parent_group\"\r\n  item:\r\n    name: \"&e示例子物品组\"\r\n    material: REDSTONE\r\n\r\nrsc_example_seasonal_group:\r\n  type: seasonal\r\n  month: 5\r\n  item:\r\n    name: \"&b示例季节性物品组\"\r\n    material: OAK_LEAVES\r\n\r\nrsc_example_locked_group:\r\n  type: locked\r\n  parents:\r\n    - slimefun:basic_machines\r\n  item:\r\n    name: \"&l示例锁定物品组\"\r\n    material: REPEATER\r\n\r\nrsc_example_tier_10086_group:\r\n  type: sub\r\n  parent: rsc_example_parent_group\r\n  tier: 10086\r\n  item:\r\n    name: \"&e示例优先级10086组\"\r\n    material: IRON_ORE\r\n\r\nrsc_example_link_group:\r\n  type: button\r\n  parent: rsc_example_parent_group\r\n  item:\r\n    name: \"&e示例链接组\"\r\n    material: COMMAND_BLOCK\r\n  actions:\r\n  - \"link https://rsc.himcs.top/#/README\"\r\n\r\nrsc_example_console_group:\r\n  type: button\r\n  parent: rsc_example_parent_group\r\n  item:\r\n    name: \"&e示例控制台指令组\"\r\n    material: BEDROCK\r\n  actions:\r\n  - \"console say 示例控制台指令组\"\r\n"));
            YamlMappingNode root = (YamlMappingNode)stream.Documents[0].RootNode;
            BuildSession session = new("TestCase", new(Environment.CurrentDirectory));
            ClassDefinition? itemGroups = new ItemGroupGenerator(root).OnGenerate(session);
            if (itemGroups != null)
                Console.WriteLine(itemGroups.BuildContent());
        }
    }
}
