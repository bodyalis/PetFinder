namespace PetFinder.Application.Providers.IFileProvider;

public record FileContent(
    Stream Stream,
    string FileName,
    string BucketName,
    IDictionary<string, string>? MetaData = null);