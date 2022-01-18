using MassTransit;
using MassTransit.Messages.Models.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Branch1.Consumers
{
    public class DecisionConsumer : IConsumer<Decision>
    {
        ILogger<DecisionConsumer> _logger;

        public DecisionConsumer()
        {
        }

        public DecisionConsumer(ILogger<DecisionConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<Decision> context)
        {
            
            if (context.GetRetryAttempt() == 0)
            {
                throw new Exception();
            }
            if (context.GetRetryAttempt() == 1)
            {
                _logger.LogInformation("Value Retry 1 Recived From Queue to Branch 1: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
                return Task.FromResult(1);
            }
            _logger.LogInformation("Value Recived From Queue to Branch 1: {from},{to},{schoolName},{Description}", context.Message.From, context.Message.To, context.Message.SchoolName, context.Message.Description);
            return Task.FromResult(1);
        }
    }
}
