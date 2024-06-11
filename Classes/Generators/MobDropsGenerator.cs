using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using rsc_converter.Classes.Yaml;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Attributes;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Script;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class MobDropsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.mobDrops", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}MobDrops");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "mob_drops.yml"))));
        YamlMappingNode mobDrops = (YamlMappingNode)stream.Documents[0].RootNode;

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

        if (mobDrops is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.mobDrops.implementations", $"{stringKey.ToUpper()}MobDropImplementation")
            {
                Super = GuguSlimefunItemClass.Class,
                Interfaces = [NotPlaceableClass.Class, RandomMobDropClass.Class]
            };
            CtorMethodDefinition ctor = new(ItemGroupClass.Class, SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ParamsClassDefinition(ItemStackClass.Class));
            ctor.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1), new ParameterValue(2), new ParameterValue(3)));
            itemClass.Ctors.Add(ctor);

            MethodDefinition preRegister = new("preRegister");
            RawValue slimefunItemStackValue = new($"{itemsClass.Name}.{stringKey.ToUpper()}");
            slimefunItemStackValue.ImportList.Import(itemsClass);

            IValue itemGroup = value.ReadItemGroup(itemGroupClass);

            bool hasScript = value.Contains("script");
            if (hasScript)
            {
                string scriptFileName = value.GetString("script")!;
                string script = File.ReadAllText(Path.Combine(session.Directory.FullName, "scripts", scriptFileName + ".js"));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetEval, new NewInstanceAction(JavaScriptEvalClass.Class, new StringValue(script))));
            }

            itemClass.Methods.Add(preRegister);

            string entityType = value.GetString("entity")!;
            RawValue entity = new($"{EntityTypeClass.Class.Name}.{entityType.ToUpper()}");
            entity.ImportList.Import(EntityTypeClass.Class);
            itemClass.FieldList.Add(new(EntityTypeClass.Class, "entity", entity)
            {
                Access = AccessAttribute.Private
            });

            IValue material = new MaterialValue($"{entityType.ToUpper()}_SPAWN_EGG");
            IValue?[] recipe = [null, null, null, new NewInstanceAction(AdvancedCustomItemStackClass.Class, material)];
            RawValue recipeType = new($"{RecipeTypeClass.Class.Name}.MOB_DROP");
            recipeType.ImportList.Import(RecipeTypeClass.Class);
            int chance = value.GetInt("chance")!;
            itemClass.FieldList.Add(new(new RawClassDefinition("int"), "chance", new NumberValue<int>(chance))
            {
                Access = AccessAttribute.Private
            });
            MethodDefinition getMobDropChance = new("getMobDropChance")
            {
                ReturnType = new RawClassDefinition("int")
            };
            getMobDropChance.Block.Actions.Add(new ReturnAction(new RawValue("chance")));
            itemClass.Methods.Add(getMobDropChance);

            itemClasses.Add(itemClass);
            onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, slimefunItemStackValue, recipeType, new MultipleValue(recipe)).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
        }

        itemClasses.Add(generated);

        return itemClasses;
    }
}
