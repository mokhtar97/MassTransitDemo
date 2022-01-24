using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api.Consumers
{
    public class StockConsumer : IConsumer<OrderCreatedEvent>
    {
        ILogger<StockConsumer> _logger;
        private readonly IPublishEndpoint publishEndpoint;

        public StockConsumer()
        {
        }

        public StockConsumer(ILogger<StockConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
        }
        public  Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {

            if (context.GetRetryAttempt() == 0)
            {
                if(context.Message.Amount>2)
                {
                     publishEndpoint.Publish<OrderFailedCreatedEvent>(new OrderFailedCreatedEvent() { Id = context.Message.OrderId });
                }
                // _logger.LogInformation("Value Retry 1 Recived From Queue to Branch 1: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
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
