using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.GuguSlimefunLib.Script;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.System;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class MobDropsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.mobDrops", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}MobDrops");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "mob_drops.yml"))));
        YamlMappingNode items = (YamlMappingNode)stream.Documents[0].RootNode;

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

        if (items is not YamlMappingNode mappingNode) return null;
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

            String entityType = value.GetString("entity")!;
            RawValue entity = new($"{EntityTypeClass.Class.Name}.{entityType.ToUpper()}");
            entity.ImportList.Import(EntityTypeClass.Class);
            itemClass.FieldList.Add(new(EntityTypeClass.Class, "entity", entity)
            {
                Access = AccessAttribute.Private
            });

            IValue material = new MaterialValue($"{entityType.ToUpper()}_SPAWN_EGG");
            IValue[] recipe = [null, null, null, new NewInstanceAction(AdvancedCustomItemStackClass.Class, material)];
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
