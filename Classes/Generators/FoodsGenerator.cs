using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using rsc_converter.Classes.Yaml;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Script;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class FoodsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "foods.yml"))));
        YamlMappingNode foods = (YamlMappingNode)stream.Documents[0].RootNode;

        IList<ClassDefinition> itemClasses = [];
        ClassDefinition itemGroupClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups")!;
        ClassDefinition recipeTypeClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes")!;
        ClassDefinition itemsClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items")!;

        MethodDefinition onSetup = itemsClass.FindMethod("onSetup")!;

        if (foods is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.items.implementations", $"{stringKey.ToUpper()}ItemImplementation")
            {
                Super = GuguSlimefunFoodClass.Class
            };
            CtorMethodDefinition ctor = new(ItemGroupClass.Class, SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ParamsClassDefinition(ItemStackClass.Class));
            ctor.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1), new ParameterValue(2), new ParameterValue(3)));
            itemClass.Ctors.Add(ctor);

            MethodDefinition preRegister = new("preRegister");
            RawValue slimefunItemStackValue = new($"{itemsClass.Name}.{stringKey.ToUpper()}");
            slimefunItemStackValue.ImportList.Import(itemsClass);

            IValue itemGroup = value.ReadItemGroup(itemGroupClass);
            IValue recipeType = value.ReadRecipeType(recipeTypeClass);
            IValue[] recipe = value.ReadRecipe(session.Directory, itemsClass);

            bool hasScript = value.Contains("script");
            if (hasScript)
            {
                string scriptFileName = value.GetString("script")!;
                string script = File.ReadAllText(Path.Combine(session.Directory.FullName, "scripts", scriptFileName + ".js"));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetEval, new NewInstanceAction(JavaScriptEvalClass.Class, new StringValue(script))));
            }

            itemClass.Methods.Add(preRegister);

            itemClasses.Add(itemClass);
            onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, slimefunItemStackValue, recipeType, new MultipleValue(recipe)).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
        }

        return itemClasses;
    }
}
