using Automatonymous;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Sagas.Order
{
    public class OrderSagaStatus : SagaStateMachineInstance
    {
        //this for Status
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }

        //this for business
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
