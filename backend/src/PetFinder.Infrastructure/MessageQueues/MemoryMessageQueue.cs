using System.Threading.Channels;
using PetFinder.Application.Messaging;

namespace PetFinder.Infrastructure.Messaging;

public class MemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task<TMessage> GetMessage(CancellationToken cancellationToken)
        => await _channel.Reader.ReadAsync(cancellationToken);

    public async Task PutMessage(TMessage message, CancellationToken cancellationToken)
        => await _channel.Writer.WriteAsync(message, cancellationToken);
}