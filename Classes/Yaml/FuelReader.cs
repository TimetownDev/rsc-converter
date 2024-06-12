using rsc_converter.Classes.Utils;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Yaml;

public static class FuelReader
{
    public static IValue[] ReadFuels(this YamlNode yaml, DirectoryInfo directory, ClassDefinition? itemsClass = null)
    {
        YamlNode fuelsNode = yaml["fuels"];
        if (fuelsNode is not YamlMappingNode mappingNode) return [];
        IList<IValue> fuels = [];
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            if (pair.Key is YamlScalarNode scalarNode)
            {
                string fuelName = scalarNode.Value!;
                IValue fuel = pair.Value.ReadFuel(directory, itemsClass);
                fuels.Add(fuel);
            }
        }

        return fuels.ToArray();
    }

    public static IValue ReadFuel(this YamlNode yaml, DirectoryInfo directory, ClassDefinition? itemsClass = null)
    {
        int seconds = yaml.GetInt("seconds");
        IValue item = yaml.ReadItem("item", directory, itemsClass);
        IValue output = new NullValue();
        try
        {
            output = yaml.ReadItem("output", directory, itemsClass);
        }
        catch { }
        return new NewInstanceAction(MachineFuelClass.Class, new NumberValue<int>(seconds), item, output);
    }
}
