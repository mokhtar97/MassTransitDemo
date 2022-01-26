using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Messages.Models.Events
{
   public class OrderShippmentStartEvent
    {
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }

        public int Amount { get; set; }
    }
}
