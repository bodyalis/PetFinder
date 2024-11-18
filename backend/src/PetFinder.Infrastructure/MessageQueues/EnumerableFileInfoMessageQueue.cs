using PetFinder.Application.Messaging;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Infrastructure.Messaging;

public class EnumerableFileInfoMessageQueue : MemoryMessageQueue<IEnumerable<FileInfo>>
{
};