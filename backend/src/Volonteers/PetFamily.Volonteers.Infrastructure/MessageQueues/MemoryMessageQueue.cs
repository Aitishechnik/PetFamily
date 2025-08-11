using PetFamily.Core.Messaging;
using System.Threading.Channels;

namespace PetFamily.Volonteers.Infrastructure.MessageQueues
{
    public class MemoryMessageQueue<TMassage> : IMessageQueue<TMassage>
    {
        private readonly Channel<TMassage> _channel
            = Channel.CreateUnbounded<TMassage>();
        public async Task WriteAsync(
            TMassage message,
            CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public async Task<TMassage> ReadAsync(CancellationToken cancellation = default)
        {
            return await _channel.Reader.ReadAsync(cancellation);
        }
    }
}
