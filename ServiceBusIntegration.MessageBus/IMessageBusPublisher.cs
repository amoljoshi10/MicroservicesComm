using System.Threading.Tasks;

namespace ServiceBusIntegration.MessageBus
{
    public interface IMessageBusPublisher
    {
        Task PublishMessage(ServiceBusBaseMessage message, string topicName);
    }
}
