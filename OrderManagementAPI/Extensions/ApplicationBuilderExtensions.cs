using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderManagementAPI.ServiceBusMessaageConsumers;

namespace OrderManagementAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceBusMessageConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzServiceBusConsumer(this IApplicationBuilder app, ILogger logger)
        {
            logger.LogInformation("OrderAPI  UseAzServiceBusConsumer method starts");
            ServiceBusConsumer = app.ApplicationServices.GetService<IServiceBusMessageConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            logger.LogInformation("OrderAPI  UseAzServiceBusConsumer method ends");
            
            return app;
        }

        private static void OnStarted()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStopping()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
