using PetFinder.Application.Features.Shared;

namespace PetFinder.API.Processors;

/// <summary>
/// Используется для удобной работы с файл-контентом, сам вызовет Dispose у стрима
/// </summary>
internal class FormFileProcessor(int fileCount = 0) : IDisposable
{
    private readonly List<FileDto> _files = new(fileCount);

    public IEnumerable<FileDto> Process(IFormFileCollection formFileCollection)
    {
        foreach (var formFile in formFileCollection)
        {
            var stream = formFile.OpenReadStream();
            FileDto dto = new FileDto(stream, formFile.FileName);
            _files.Add(dto);
        }

        return _files;
    }


    public void Dispose() => _files.ForEach(f => f.Content.Dispose());
}