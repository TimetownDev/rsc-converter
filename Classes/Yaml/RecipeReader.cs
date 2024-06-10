using rsc_converter.Classes.Utils;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Yaml;

public static class RecipeReader
{
    public static IValue[] ReadRecipe(this YamlNode yaml, DirectoryInfo directory, ClassDefinition itemsClass)
    {
        IValue[] recipe = new IValue[9];

        bool hasRecipe = yaml.Contains("recipe");
        if (hasRecipe)
        {
            foreach (KeyValuePair<YamlNode, YamlNode> recipePair in (YamlMappingNode)yaml["recipe"])
            {
                int index = int.Parse(((YamlScalarNode)recipePair.Key).Value!);
                if (index > 9)
                    throw new ArgumentException("配方序号不能大于9");
                if (index <= 0)
                    throw new ArgumentException("配方序号不能小于等于0");
                IValue item = yaml["recipe"].ReadItem(index.ToString(), directory, itemsClass);
                recipe[index - 1] = item;
            }
        }

        return recipe;
    }

    public static IValue ReadRecipeType(this YamlNode yaml, ClassDefinition recipeTypeClass)
    {
        string recipeTypeId = yaml.GetString("recipe_type", "NULL").ToUpper();
        RawValue recipeType;
        FieldDefinition? recipeTypeField = recipeTypeClass.FieldList.FindField(recipeTypeId);
        if (recipeTypeField == null)
        {
            recipeType = new RawValue($"{RecipeTypeClass.Class.Name}.{recipeTypeId}");
            recipeType.ImportList.Import(RecipeTypeClass.Class);
        }
        else
        {
            recipeType = new RawValue($"{recipeTypeClass.Name}.{recipeTypeId}");
            recipeType.ImportList.Import(recipeTypeClass);
        }

        return recipeType;
    }
}
