using System.Collections;

namespace PetFinder.Domain.Shared.ValueObjects;

public record ValueObjectList<T> : IReadOnlyList<T>
{
    private ValueObjectList()
    {
    }
    
    public ValueObjectList(IEnumerable<T> values)
    {
        Values = values.ToList().AsReadOnly();
    }

    public IReadOnlyList<T> Values { get; } = default!;

    public int Count => Values.Count;

    public T this[int index] => Values[index];

    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public static implicit operator List<T>(ValueObjectList<T> valueObjectList)
        => valueObjectList.Values.ToList();
    
    public static implicit operator ValueObjectList<T>(List<T> values)
        => new(values);
}
