using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;

public interface IDestinationSender : IDisposable
{
    public void Send(Destination destination, string message);
}
