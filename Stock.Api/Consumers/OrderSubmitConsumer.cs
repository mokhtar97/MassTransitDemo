using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api.Consumers
{
    public class OrderSubmitConsumer : IConsumer<OrderSubmitCreatedEvent>
    {
        private readonly ILogger<OrderSubmitConsumer> _logger;
        private readonly IPublishEndpoint publishEndpoint;
        public OrderSubmitConsumer()
        {
        }

        public OrderSubmitConsumer(ILogger<OrderSubmitConsumer> logger, IPublishEndpoint _publishEndpoint)
        {
            _logger = logger;
            publishEndpoint = _publishEndpoint;
        }
        public  Task Consume(ConsumeContext<OrderSubmitCreatedEvent> context)
        {

            if (context.GetRetryAttempt() == 0)
            {

                if (context.Message.Amount > 2)
                {
                    publishEndpoint.Publish<OrderSubmittedFailedEvent>(new OrderSubmittedFailedEvent() {  OrderId = context.Message.OrderId});
                }
                else
                {
                    publishEndpoint.Publish<OrderSubmittedSuccessfullyEvent>(new OrderSubmittedSuccessfullyEvent() { OrderId = context.Message.OrderId });
                }
                return Task.FromResult(1);
            }
            if (context.GetRetryAttempt() == 1)
            {
              
                return Task.FromResult(1);
            }
            
            return Task.FromResult(1);
        }

        
    }
}