using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Core
{
    public class ConsumerBaseEntity
    {
        public Type ConsumerType { get; set; }
        public string InstanceId { get; set; }
    }
}
