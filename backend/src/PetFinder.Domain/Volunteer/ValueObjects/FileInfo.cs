using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteer.ValueObjects;

public record FileInfo
{
    private FileInfo()
    {
    }

    private FileInfo(string path, string name)
    {
        Path = path;
        Name = name;
    }

    public string Path { get; } = default!;

    public string Name { get; } = default!;

    public static Result<FileInfo, Error> Create(string path, string name)
    {
        var validationResult = Validate(path, name);
        
        return validationResult.IsFailure 
            ? validationResult.Error 
            : new FileInfo(path, name);
    }

    public static UnitResult<Error> Validate(string path, string name)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > Constants.FileExtension.MaxPathLength)
            return Errors.General.ValueIsInvalid(
                nameof(Path),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.FileExtension.MaxPathLength));
        
        var nameValidationResult = ValidateName(name);
        
        return nameValidationResult.IsFailure 
            ? nameValidationResult.Error 
            : UnitResult.Success<Error>();
    }

    public static UnitResult<Error> ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.FileExtension.MaxNameLength)
            return Errors.General.ValueIsInvalid(
                nameof(Name),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.FileExtension.MaxNameLength));

        string extension = System.IO.Path.GetExtension(name);
        
        if (!Constants.FileExtension.Images.Contains(extension))
            return Errors.General.ValueIsInvalid(nameof(Name), "Extension of file is not supported");

        return UnitResult.Success<Error>();
    }
}