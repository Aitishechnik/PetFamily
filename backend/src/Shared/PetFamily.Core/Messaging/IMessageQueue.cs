namespace PetFamily.Core.Messaging
{
    public interface IMessageQueue<TMessage>
    {
        Task WriteAsync(TMessage path, CancellationToken cancellationToken = default);
        Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
    }
}
