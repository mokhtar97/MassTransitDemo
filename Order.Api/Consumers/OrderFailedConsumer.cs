using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.Extensions.Logging;
using Order.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Consumers
{
    public class OrderFailedConsumer: IConsumer<OrderFailedCreatedEvent>
    {
        ILogger<OrderFailedConsumer> _logger;
        private readonly OrderContext orderContext;

        public OrderFailedConsumer()
    {

    }

    public OrderFailedConsumer(ILogger<OrderFailedConsumer> logger, OrderContext _orderContext)
    {
        _logger = logger;
            orderContext = _orderContext;
        }
    public Task Consume(ConsumeContext<OrderFailedCreatedEvent> context)
    {

        if (context.GetRetryAttempt() == 0)
        {
                //if (context.Message != null )
                //{
                //    var order = orderContext.orders.FirstOrDefault(o => o.Id == context.Message.Id);
                //    orderContext.orders.Remove(order);
                //    orderContext.SaveChanges();
                //}
               
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
