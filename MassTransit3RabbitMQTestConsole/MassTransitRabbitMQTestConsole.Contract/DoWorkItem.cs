using MassTransit;
using System;

namespace MassTransitRabbitMQTestConsole.Contract
{
    public class DoWorkItem
    {
        public DoWorkItem()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; private set; }
        public string Text { get; set; }
    }
}
