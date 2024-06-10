using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Values;
using rsc_converter.Classes.Interfaces;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.System;

namespace rsc_converter.Classes.Generators;

public class PluginMainGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}", $"{char.ToUpper(session.Name[0]) + session.Name[1..]}Main")
        {
            Super = JavaPluginClass.Class,
            Interfaces = [SlimefunAddonClass.Class]
        };
        MethodDefinition onEnable = new("onEnable");
        MethodDefinition onDisable = new("onDisable");
        MethodDefinition getJavaPlugin = new("getJavaPlugin")
        {
            ReturnType = JavaPluginClass.Class
        };
        MethodDefinition getBugTrackerURL = new("getBugTrackerURL")
        {
            ReturnType = SystemClass.StringClass
        };


        foreach (ClassDefinition classDefinition in session.ClassDefinitions)
        {
            MethodDefinition? onSetup = classDefinition.FindMethod("onSetup");
            if (onSetup == null) continue;

            onEnable.Block.Actions.Add(classDefinition.Invoke(onSetup, new ThisValue()));
        }
        getJavaPlugin.Block.Actions.Add(new ReturnAction(new ThisValue()));
        getBugTrackerURL.Block.Actions.Add(new ReturnAction());


        generated.Methods.Add(onEnable);
        generated.Methods.Add(onDisable);
        generated.Methods.Add(getJavaPlugin);
        generated.Methods.Add(getBugTrackerURL);

        return [generated];
    }
}
