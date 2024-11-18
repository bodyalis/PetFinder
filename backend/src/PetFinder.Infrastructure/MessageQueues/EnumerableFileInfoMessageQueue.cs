using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Infrastructure.MessageQueues;

public class EnumerableFileInfoMessageQueue : MemoryMessageQueue<IEnumerable<FileInfo>>
{
};