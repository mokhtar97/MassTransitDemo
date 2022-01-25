using Automatonymous;
using MassTransit.Messages.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Sagas.Order
{
    public class OrderSagaMachine : MassTransitStateMachine<OrderSagaStatus>
    {
        public OrderSagaMachine()
        {
            InstanceState(s => s.CurrentState,Active);

            Event(() => OrderCreate, x =>
            x.CorrelateBy(order => order.OrderId.ToString(),
            context => context.Message.OrderId.ToString())
            .SelectId(context => Guid.NewGuid()));



            Event(() => OrderFailed, x => x.CorrelateById(context =>
                context.Message.CorrelationId));


            Event(() => OrderSuccess, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => OrderSubmitted, x => x.CorrelateById(context =>
               context.Message.CorrelationId));
            
            Initially(
               When(OrderSuccess)
                  //.Then(context =>
                  //{
                  //    // Will be done by auto mapper                                      
                  //    context.Instance.OrderId = context.Data.OrderId;
                  //    context.Instance.Name = context.Data.Name;
                  //    context.Instance.Amount = context.Data.Amount;
                  //})
                  // If(context => context.Data.Amount == 0, x =>
                  .Publish(context => new OrderSubmitCreatedEvent())
                   .TransitionTo(Active)
               //When(OrderFailed)
               //.Publish(context => new OrderFailedCreatedEvent())
               //    .TransitionTo(Active)
                   );

            //During Not Working Fine With Us.
            #region During
            During(Active,
    
                When(OrderSuccess)
                    .Then(context => context.Instance.OrderCreatedDate = DateTime.Now)
                    .ThenAsync(
                        context => Console.Out.WriteLineAsync(
                            $"Order processed. Id: {context.Instance.CorrelationId}"))
                 .Publish(context => new OrderSuccessCreatedEvent() { name = "mokhtarsd From Success" }),
                 When(OrderSubmitted)
                    .Then(context => context.Instance.OrderCreatedDate = DateTime.Now)
                    .ThenAsync(
                        context => Console.Out.WriteLineAsync(
                            $"Order processed. Id: {context.Instance.CorrelationId}"))
                 .Publish(context => new OrderFailedCreatedEvent() { Name = "mokhtarsd From Submitt Success" }),
                When(OrderFailed)
                    .Then(context => context.Instance.OrderFailedCreatedDate = DateTime.Now)
                    .ThenAsync(
                        c => Console.Out.WriteLineAsync(
                            $"Order failed for amount: {c.Instance.Amount} Id:{c.Instance.CorrelationId}"))
                    .Publish(context => new OrderFailedCreatedEvent() { Name = "mokhtarsd From Failed", Amount=55 })
              
                    .Finalize());
            #endregion

            SetCompletedWhenFinalized();
        }

        public State ProcessOrder { get; private set; }
        public State Active { get; private set; }
        public Event<OrderCreatedEvent> OrderCreate { get; private set; }

        public Event<OrderFailedCreatedEvent> OrderFailed { get; private set; }
        
        public Event<OrderSuccessCreatedEvent> OrderSuccess { get; private set; }
        public Event<OrderSubmitCreatedEvent> OrderSubmitCreate { get; private set; }
        public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; }
    }
}
