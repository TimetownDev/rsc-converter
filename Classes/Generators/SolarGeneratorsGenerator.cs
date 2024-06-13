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

public class SolarGeneratorsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.generators", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}SolarGenerators");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "solar_generators.yml"))));
        YamlMappingNode generators = (YamlMappingNode)stream.Documents[0].RootNode;

        IList<ClassDefinition> itemClasses = [];
        ClassDefinition itemGroupClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups")!;
        ClassDefinition recipeTypeClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes")!;
        ClassDefinition itemsClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items")!;

        MethodDefinition onSetup = new("onSetup")
        {
            ParameterTypes = [SlimefunAddonClass.Class],
            IsStatic = true
        };
        generated.Methods.Add(onSetup);

        if (generators is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.generators.implementations", $"{stringKey.ToUpper()}SolarGeneratorImplementation")
            {
                Super = GuguSolarGeneratorClass.Class,
            };
            CtorMethodDefinition ctor = new(ItemGroupClass.Class, new RawClassDefinition("int"), new RawClassDefinition("int"), SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ArrayClassDefinition(ItemStackClass.Class), new RawClassDefinition("int"));
            ctor.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1), new ParameterValue(2), new ParameterValue(3), new ParameterValue(4), new ParameterValue(5), new ParameterValue(6)));
            itemClass.Ctors.Add(ctor);

            MethodDefinition preRegister = new("preRegister");

            RawValue slimefunItemStackValue = new($"{itemsClass.Name}.{stringKey.ToUpper()}");
            slimefunItemStackValue.ImportList.Import(itemsClass);

            IValue itemGroup = value.ReadItemGroup(itemGroupClass);
            IValue recipeType = value.ReadRecipeType(recipeTypeClass);
            IValue[] recipe = value.ReadRecipe(session.Directory, itemsClass);

            int capacity = value.GetInt("capacity");
            int dayEnergy = value.GetInt("dayEnergy");
            int nightEnergy = value.GetInt("nightEnergy");
            int lightLevel = value.GetInt("lightLevel");

            preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSolarGeneratorClass.SetLightLevel, new NumberValue<int>(lightLevel)));

            itemClass.Methods.Add(preRegister);

            itemClasses.Add(itemClass);
            onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, new NumberValue<int>(dayEnergy), new NumberValue<int>(nightEnergy), slimefunItemStackValue, recipeType, new ArrayValue(ItemStackClass.Class, recipe), new NumberValue<int>(capacity)).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
        }

        itemClasses.Add(generated);

        return itemClasses;
    }
}
