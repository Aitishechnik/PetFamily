using System.Threading.Channels;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.MessageQueues
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
