using rsc_converter.JavaGenerator.Exceptions;
using System.Collections;
using System.Text;

namespace rsc_converter.JavaGenerator;

public class FieldList : IList<FieldDefinition>
{
    private readonly IList<FieldDefinition> fields = [];

    public FieldDefinition this[int index] { get => fields[index]; set => fields[index] = value; }

    public int Count => fields.Count;

    public bool IsReadOnly => fields.IsReadOnly;

    public void Add(FieldDefinition item)
    {
        if (Contains(item)) return;
        fields.Add(item);
    }

    public void Clear()
    {
        fields.Clear();
    }

    public bool Contains(FieldDefinition item)
    {
        return fields.Any(x => x == item);
    }

    public void CopyTo(FieldDefinition[] array, int arrayIndex)
    {
        fields.CopyTo(array, arrayIndex);
    }

    public IEnumerator<FieldDefinition> GetEnumerator()
    {
        return fields.GetEnumerator();
    }

    public int IndexOf(FieldDefinition item)
    {
        for (int i = 0; i < fields.Count; i++)
        {
            if (fields[i] == item) return i;
        }
        return -1;
    }

    public void Insert(int index, FieldDefinition item)
    {
        if (Contains(item)) return;
        fields.Insert(index, item);
    }

    public bool Remove(FieldDefinition item)
    {
        for (int i = 0; i < fields.Count; i++)
        {
            if (fields[i] == item)
            {
                fields.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        fields.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)fields).GetEnumerator();
    }
    public string BuildContent(ClassDefinition classDefinition)
    {
        StringBuilder sb = new();

        int x = 0;
        foreach (FieldDefinition field in fields)
        {
            sb.Append(field.BuildContent(classDefinition));
            sb.Append(';');
            if (x + 1 < fields.Count)
                sb.Append('\n');
            x++;
        }

        return sb.ToString();
    }

    public FieldDefinition? FindField(string name, bool throwException = false)
    {
        foreach (FieldDefinition field in fields)
        {
            if (field.Name == name) return field;
        }

        if (throwException)
            throw new NoSuchFieldException(name);

        return null;
    }
}
