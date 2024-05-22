namespace rscconventer.JavaGenerator.Utils;

public static class ImportUtils
{
    public static void Import(IList<ClassDefinition> importList, ClassDefinition classDefinition)
    {
        if (CanImport(importList, classDefinition))
        {
            importList.Add(classDefinition);
        }
    }
    public static bool CanImport(IList<ClassDefinition> importList, ClassDefinition classDefinition)
    {
        foreach (ClassDefinition definition in importList)
        {
            if (definition.Name == classDefinition.Name) return false;
        }

        return true;
    }
    public static string GetUsing(IList<ClassDefinition> importList, ClassDefinition classDefinition)
    {
        if (importList.Contains(classDefinition)) return classDefinition.Name;
        return classDefinition.Namespace + "." + classDefinition.Name;
    }
}
