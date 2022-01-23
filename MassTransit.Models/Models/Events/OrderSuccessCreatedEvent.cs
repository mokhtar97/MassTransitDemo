using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Messages.Models.Events
{
    public class OrderSuccessCreatedEvent
    {
        public int Id { get; set; }
        public string name { get; set; }
    }
}
