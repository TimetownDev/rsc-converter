using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;

namespace rscconventer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ClassDefinition sfItem = new("io.github.thebusybiscuit.slimefun4.api.items", "SlimefunItem");
            ClassDefinition classDefinition = new("me.ddggdd135", "testClass")
            {
                Super = sfItem
            };
            MethodDefinition methodDefinition = new("testMethod");
            methodDefinition.ParameterTypes.Add(SystemClass.StringClass);
            methodDefinition.Block.Actions.Add(new ObjectInvokeAction(new StaticInvokeAction(BukkitClass.Class, BukkitClass.GetConsoleSender), ConsoleCommandSenderClass.SendMessage, new ParameterValue(0)));
            classDefinition.Methods.Add(methodDefinition);

            classDefinition.Attributes.Add(JavaGenerator.Attributes.ClassAttribute.Abstract);
            Console.WriteLine(classDefinition.ToString());
        }
    }
}
