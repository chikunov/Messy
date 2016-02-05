using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MassTransit.MessageData;

namespace Messy.MassTransitIntegration.Services
{
    internal class MessageDataRepository : IMessageDataRepository
    {
        public Task<Stream> Get(Uri address, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<Uri> Put(Stream stream, TimeSpan? timeToLive = null, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}
