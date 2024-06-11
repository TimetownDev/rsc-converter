using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using rsc_converter.Classes.Yaml;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Script;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Utils;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.System;
using rsc_converter.JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class MenusGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.menus", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}Menus");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "menus.yml"))));
        YamlNode menus = stream.Documents[0].RootNode;

        IList<ClassDefinition> menuClasses = [];
        ClassDefinition itemsClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items")!;

        if (menus is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition menuClass = new($"me.ddggdd135.{session.Name}.menus.implementations", $"{stringKey.ToUpper()}MenuImplementation")
            {
                Super = MachineMenuClass.Class
            };
            CtorMethodDefinition ctor1 = new(SystemClass.StringClass);
            CtorMethodDefinition ctor2 = new(SystemClass.StringClass, BlockMenuPresetClass.Class);
            ctor1.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0)));
            ctor2.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1)));
            menuClass.Ctors.Add(ctor1);
            menuClass.Ctors.Add(ctor2);

            MethodDefinition init = new("init");

            string title = value.GetString("title")!;
            bool hasScript = value.Contains("script");
            if (hasScript)
            {
                string scriptFileName = value.GetString("script")!;
                string script = File.ReadAllText(Path.Combine(session.Directory.FullName, "scripts", scriptFileName + ".js"));
                init.Block.Actions.Add(new ThisValue().Invoke(MachineMenuClass.SetEval, new NewInstanceAction(JavaScriptEvalClass.Class, new StringValue(script))));
            }

            int progress = 22;
            IValue progressItem = new NewInstanceAction(ItemStackClass.Class, new MaterialValue("IRON_PICKAXE"));
            if (value.Contains("slots"))
            {
                YamlNode slots = value["slots"];
                if (slots is not YamlMappingNode slotsMappingNode) continue;
                foreach (KeyValuePair<YamlNode, YamlNode> slotPair in slotsMappingNode)
                {
                    YamlNode rangeNode = slotPair.Key;
                    if (rangeNode is not YamlScalarNode rangeScalarNode) continue;
                    string? rangeString = rangeScalarNode.Value;
                    if (!Range.IsRange(rangeString)) continue;

                    YamlNode slot = slotPair.Value;

                    Range range = Range.Parse(rangeString!);
                    IValue itemStack = slot.ReadItem(session.Directory, itemsClass);
                    bool isProgressBar = slot.GetBoolean("progressbar", false);
                    if (isProgressBar)
                    {
                        progress = range.Start;
                        if (slot.Contains("progressBarItem"))
                            progressItem = slots.ReadItem("progressBarItem", session.Directory, itemsClass);
                        else
                            progressItem = itemStack;
                    }
                    else if (range.ToList().Contains(progress))
                    {
                        progressItem = itemStack;
                    }
                    init.Block.Actions.Add(new ThisValue().Invoke(MachineMenuClass.AddItemEx, itemStack, new MultipleValue(range.ToList().Select<int, IValue?>(x => new NumberValue<int>(x)).ToList())));
                }
            }

            init.Block.Actions.Add(new ThisValue().Invoke(MachineMenuClass.SetProgressSlot, new NumberValue<int>(progress)));
            init.Block.Actions.Add(new ThisValue().Invoke(MachineMenuClass.SetProgressBar, progressItem));

            menuClass.Methods.Add(init);

            menuClasses.Add(menuClass);
            FieldDefinition field = new(menuClass, stringKey.ToUpper())
            {
                IsStatic = true
            };

            bool isImport = value.Contains("import");
            if (isImport)
            {
                string importId = value.GetString("import")!;
                field.DefaultValue = new NewInstanceAction(menuClass, new StringValue(title), BlockMenuPresetUtilsClass.Class.Invoke(BlockMenuPresetUtilsClass.FindBlockMenuPreset, new StringValue(importId)));
            }
            else
                field.DefaultValue = new NewInstanceAction(menuClass, new StringValue(title));
            generated.FieldList.Add(field);
        }

        menuClasses.Add(generated);
        return menuClasses;
    }
}
