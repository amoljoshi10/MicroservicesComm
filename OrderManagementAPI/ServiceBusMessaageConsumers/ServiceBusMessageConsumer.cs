using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.ServiceBusMessages;
using ServiceBusIntegration.MessageBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagementAPI.ServiceBusMessaageConsumers
{
    public class ServiceBusMessageConsumer : IServiceBusMessageConsumer
    {
        private readonly string subscriptionName = "onlineshoppingapporder";
        private readonly IReceiverClient checkoutMessageReceiverClient;
        private readonly IReceiverClient orderPaymentUpdateMessageReceiverClient;

        private readonly IConfiguration _configuration;

        private readonly IOrderService _orderService;
        private readonly IMessageBusPublisher _messageBus;
        private readonly ILogger<ServiceBusMessageConsumer> _logger;

        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentRequestMessageTopic;
        private readonly string orderPaymentUpdatedMessageTopic;

        public ServiceBusMessageConsumer(IConfiguration configuration, IMessageBusPublisher messageBus,
                                        IOrderService orderService,ILogger<ServiceBusMessageConsumer> logger)
        {
            _configuration = configuration;
            _orderService = orderService;
            _logger = logger;
            _messageBus = messageBus;

            var serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            checkoutMessageTopic = _configuration.GetValue<string>("CartCheckoutMessageTopic");
            _logger.LogInformation("OrderAPI ServiceBusMessageConsumer serviceBusConnectionString {serviceBusConnectionString} ", serviceBusConnectionString);
            _logger.LogInformation("OrderAPI ServiceBusMessageConsumer checkoutMessageTopic {checkoutMessageTopic} ", checkoutMessageTopic);
            checkoutMessageReceiverClient = new SubscriptionClient(serviceBusConnectionString, checkoutMessageTopic, subscriptionName);
            _logger.LogInformation("OrderAPI  checkoutMessageReceiverClient created");
        }

        public void Start()
        {
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer Start method started");
            var messageHandlerOptions = new MessageHandlerOptions(OnServiceBusException) { MaxConcurrentCalls = 4 };
            _logger.LogInformation("OrderAPI  messageHandlerOptions created");
            checkoutMessageReceiverClient.RegisterMessageHandler(OnCheckoutMessageReceived, messageHandlerOptions);
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer Start method ended");
        }

        private async Task<int> OnCheckoutMessageReceived(Message message, CancellationToken arg2)
        {
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer OnCheckoutMessageReceived method started");
            var body = Encoding.UTF8.GetString(message.Body);//json from service bus

            //save order with status not paid
            CheckoutMessage basketCheckoutMessage = JsonConvert.DeserializeObject<CheckoutMessage>(body);

            Guid orderId = Guid.NewGuid();

            Order order = new Order
            {
                UserId = basketCheckoutMessage.UserId,
                OrderDateTime = DateTime.Now,
                TotalAmount = basketCheckoutMessage.TotalAmount
            };
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer before CreateOrder call");

            return  _orderService.CreateOrder(order);
            
        }

        

        private Task OnServiceBusException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs);
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer OnServiceBusException method started");
            if(exceptionReceivedEventArgs!=null && exceptionReceivedEventArgs.Exception !=null )
            {
                _logger.LogInformation("OrderAPI ServiceBusMessageConsumer OnServiceBusException exceptionReceivedEventArgs {exceptionReceivedEventArgs} ", exceptionReceivedEventArgs.Exception.Message);
                _logger.LogInformation("OrderAPI ServiceBusMessageConsumer OnServiceBusException ExceptionMessage {exceptionReceivedEventArgs.Exception.StackTrace} ", exceptionReceivedEventArgs.Exception.StackTrace);
            }
            if (exceptionReceivedEventArgs != null && exceptionReceivedEventArgs.Exception != null &&  exceptionReceivedEventArgs.Exception.InnerException != null)
            {
                _logger.LogInformation("OrderAPI ServiceBusMessageConsumer OnServiceBusException InnerException {exceptionReceivedEventArgs.Exception.InnerException} ", exceptionReceivedEventArgs.Exception.InnerException.Message);
            }
            return Task.CompletedTask;
        }

        public void Stop()
        {
            _logger.LogInformation("OrderAPI  ServiceBusMessageConsumer Stop method called");
        }
    }
}
