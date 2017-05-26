using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MassTransit;
using MassTransitRabbitMQTestConsole.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenPipes;

namespace MassTransitRabbitMQTestConsole.Consumer
{
    public class TestMessageConsumer : IConsumer<DoWorkItem>
    {
        public Task Consume(ConsumeContext<DoWorkItem> context)
        {
            Console.WriteLine("Timestamp: " + DateTime.Now.ToString() + ", Retry Count:" + context.GetRetryAttempt());

            //if (context.GetRetryAttempt() < 3)
            //{
                //throw new InvalidCastException("aaaaa");
            //}

            Console.WriteLine(context.Message.Text);

            return Task.FromResult(0);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                x.UseConcurrencyLimit(1);
                //e.UseRetry(Retry.Interval(10, TimeSpan.FromSeconds(10));
                x.UseRetry(Retry.Incremental(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10)));
                x.ReceiveEndpoint(host, "MassTransit_Subscriber", e =>
                {
                    e.Consumer<TestMessageConsumer>();
                });
            });
            bus.Start();
            Console.WriteLine("Started listening...");
            Console.ReadKey();
            bus.Stop();
        }
    }
}
