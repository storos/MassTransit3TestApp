using GreenPipes;
using MassTransit;
using MassTransitRabbitMQTestConsole.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            //DoRegisteration();

            var bus = Bus.Factory.CreateUsingRabbitMq(x => 
            {
              var host = x.Host(new Uri("rabbitmq://localhost"), h => 
              {
                  h.Username("guest");
                  h.Password("guest");
              });
              //x.ReceiveEndpoint(host, "MassTransit_Publisher", e =>
              //  e.Consumer<DoWorkItem>());
            });
            bus.Start();
            var text = "";

            int i = 0;
            while (text != "quit")
            {
                Console.Write("Enter a message: ");
                text = Console.ReadLine();

                var message = new DoWorkItem()
                {
                    Text = string.Format("Mesaj #{0} : {2} {1}!", i, DateTime.Now, text)
                };

                bus.Publish<DoWorkItem>(message).Wait();

                i += 1;
            }

            bus.Stop();
        }

        private static void DoRegisteration()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("root");
                    h.Password("1q2w3eASD");
                });
                x.ReceiveEndpoint(host, "MassTransit_Subscriber", e => 
                {
                    e.Consumer<TestMessageConsumer>();
                    e.Consumer<DisplayMessageConsumer>();
                });
            });
            bus.Start();
            bus.Stop();
        }

        public class TestMessageConsumer : IConsumer<DoWorkItem>
        {
            public Task Consume(ConsumeContext<DoWorkItem> context)
            {
                throw new NotImplementedException();
            }
        }
        public class DisplayMessageConsumer : IConsumer<DoWorkItem>
        {
            public Task Consume(ConsumeContext<DoWorkItem> context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
