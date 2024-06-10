using rsc_converter.JavaGenerator.Interfaces;
using System.Collections;
using System.Text;

namespace rsc_converter.JavaGenerator;

public class ImportList : IList<string>
{
    private readonly IList<string> imports = [];

    public string this[int index] { get => imports[index]; set => imports[index] = value; }

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(IClassDefinition item)
    {
        imports.Add(item.FullName);
    }

    public void Add(string item)
    {
        imports.Add(item);
    }

    public void Clear()
    {
        imports.Clear();
    }

    public bool Contains(IClassDefinition item)
    {
        return imports.Contains(item.FullName);
    }

    public bool Contains(string item)
    {
        return imports.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        imports.CopyTo(array, arrayIndex);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return imports.GetEnumerator();
    }

    public int IndexOf(ClassDefinition item)
    {
        return imports.IndexOf(item.FullName);
    }

    public int IndexOf(string item)
    {
        return imports.IndexOf(item);
    }

    public void Insert(int index, ClassDefinition item)
    {
        imports.Insert(index, item.FullName);
    }

    public void Insert(int index, string item)
    {
        imports.Insert(index, item);
    }

    public bool Remove(ClassDefinition item)
    {
        return imports.Remove(item.FullName);
    }

    public bool Remove(string item)
    {
        return imports.Remove(item);
    }

    public void RemoveAt(int index)
    {
        imports.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)imports).GetEnumerator();
    }

    public void Import(IClassDefinition classDefinition)
    {
        if (CanImport(classDefinition))
        {
            Add(classDefinition);
        }
    }
    public bool CanImport(IClassDefinition classDefinition)
    {
        if (!classDefinition.NeedImport) return false;
        foreach (string fullName in imports)
        {
            if (GetName(fullName) == classDefinition.Name) return false;
        }

        return true;
    }
    public string GetUsing(IClassDefinition classDefinition)
    {
        if (Contains(classDefinition)) return classDefinition.Name;
        return classDefinition.FullName;
    }
    public void Merge(ImportList imports)
    {
        foreach (string fullName in imports)
        {
            if (!Contains(fullName)) Add(fullName);
        }
    }
    public virtual string BuildContent()
    {
        StringBuilder sb = new();

        int x = 0;
        foreach (string fullName in imports)
        {
            sb.Append("import");
            sb.Append(' ');
            sb.Append(fullName);
            sb.Append(';');
            if (x + 1 < imports.Count)
                sb.Append('\n');
            x++;
        }

        return sb.ToString();
    }
    private static string GetName(string fullName)
    {
        return fullName.Split('.')[^1];
    }
}
