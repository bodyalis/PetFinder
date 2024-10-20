using System.Collections;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Shared;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;
    public ErrorList(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        _errors = errors.ToList();
    }
    
    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> errors)
    {
        return new ErrorList(errors);
    }

    public static implicit operator ErrorList(Error error)
    {
        return new ErrorList([error]);
    }
}