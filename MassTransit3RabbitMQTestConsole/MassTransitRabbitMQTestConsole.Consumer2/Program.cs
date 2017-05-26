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
    public class DisplayMessageConsumer : IConsumer<DoWorkItem>
    {
        public Task Consume(ConsumeContext<DoWorkItem> context)
        {
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
                    h.Username("root");
                    h.Password("1q2w3eASD");
                });
                x.UseRetry(Retry.Immediate(1));
                x.ReceiveEndpoint(host, "MassTransit_Subscriber2", e => 
                {
                  e.Consumer<DisplayMessageConsumer>();
                });
            });
            bus.Start();
            Console.WriteLine("Started listening...");
            Console.ReadKey();
            bus.Stop();
        }
    }
}
