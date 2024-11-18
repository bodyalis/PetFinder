namespace PetFinder.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    Task<TMessage> GetMessage(CancellationToken cancellationToken);
    Task PutMessage(TMessage message, CancellationToken cancellationToken);
}