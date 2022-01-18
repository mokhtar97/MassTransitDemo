﻿using MassTransit;
using MassTransit.Messages.Models.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Branch2.Consumer
{
    public class DecisionConsumer : IConsumer<Decision>
    {
        ILogger<DecisionConsumer> _logger;
        public DecisionConsumer(ILogger<DecisionConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<Decision> context)
        {
            _logger.LogInformation("Value Recived From Queue to Branch 2: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
            return Task.FromResult(1);
        }
    }
}
