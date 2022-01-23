﻿using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api.Consumers
{
    public class OrderSuccess : IConsumer<OrderSuccessCreatedEvent>
    {
        ILogger<OrderSuccess> _logger;

        public OrderSuccess()
        {
        }

        public OrderSuccess(ILogger<OrderSuccess> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<OrderSuccessCreatedEvent> context)
        {

            if (context.GetRetryAttempt() == 0)
            {
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
