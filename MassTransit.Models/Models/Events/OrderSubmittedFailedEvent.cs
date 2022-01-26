using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Messages.Models.Events
{
   public class OrderSubmittedFailedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
