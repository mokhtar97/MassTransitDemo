using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippment.Consumers
{
    public class OrderShippmentConsumer : IConsumer<OrderShippmentStartEvent>
    {
        private readonly ILogger<OrderShippmentConsumer> _logger;
        private readonly IPublishEndpoint publishEndpoint;
        public OrderShippmentConsumer()
        {
        }

        public OrderShippmentConsumer(ILogger<OrderShippmentConsumer> logger, IPublishEndpoint _publishEndpoint)
        {
            _logger = logger;
            publishEndpoint = _publishEndpoint;
        }
        public Task Consume(ConsumeContext<OrderShippmentStartEvent> context)
        {

            if (context.GetRetryAttempt() == 0)
            {

                //if (context.Message.Amount < 2)
                //{
                //    publishEndpoint.Publish<OrderShippedSuccessfullyEvent>(new OrderShippedSuccessfullyEvent() { CorrelationId = context.Message.CorrelationId });
                //}
                //else
                //{
                //    publishEndpoint.Publish<OrderShippedFailedEvent>(new OrderShippedFailedEvent() { CorrelationId = context.Message.CorrelationId });
                //}
                publishEndpoint.Publish<OrderShippedFailedEvent>(new OrderShippedFailedEvent() { OrderId = context.Message.OrderId});
               
                return Task.FromResult(1);
            }
            if (context.GetRetryAttempt() == 1)
            {
                //  _logger.LogInformation("Value Retry 1 Recived From Queue to Branch 1: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
                return Task.FromResult(1);
            }
            // _logger.LogInformation("Value Recived From Queue to Branch 1: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
            return Task.FromResult(1);
        }

    }
}
