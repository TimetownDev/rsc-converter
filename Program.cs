using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;

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

            
            Console.WriteLine(classDefinition.BuildContent());
        }
    }
}
