using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Messages.Models.Events
{
   public class OrderCreatedEvent
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

       
    }
}
