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
            InstanceState(s => s.CurrentState);

            Event(() => OrderCreate, x =>
            x.CorrelateBy(order => order.OrderId.ToString(),
            context => context.Message.ToString())
            .SelectId(context => Guid.NewGuid()));

            Event(() => OrderFailed, x =>
            x.CorrelateBy(order => order.OrderId.ToString(),
            context => context.Message.ToString()));


            Event(() => OrderSuccess, x =>
           x.CorrelateBy(order => order.OrderId.ToString(),
           context => context.Message.ToString()));
          
            Initially(
               When(OrderCreate)
               .Then(context =>
               {
                   // Will be done by auto mapper                                      
                   context.Instance.OrderId = context.Data.OrderId;
                   context.Instance.Name = context.Data.Name;
                   context.Instance.Amount = context.Data.Amount;
               }).
               If(context => context.Data.Amount != 0, x =>
                    //x.Publish(context => new OrderCreatedEvent())
                    x.TransitionTo(ProcessOrder)));

            //During Not Working Fine With Us.
            //During(ProcessOrder,
            //    When(OrderSuccess)
            //        .Then(context => context.Instance.OrderCreatedDate = DateTime.Now)
            //        .ThenAsync(
            //            context => Console.Out.WriteLineAsync(
            //                $"Order processed. Id: {context.Instance.CorrelationId}"))
            //     .Publish(context => new OrderSuccessCreatedEvent() { name = "mokhtarsd" }),
            //     When(OrderFailed)
            //        .Then(context => context.Instance.OrderFailedCreatedDate = DateTime.Now)
            //        .ThenAsync(
            //            c => Console.Out.WriteLineAsync(
            //                $"Order failed for amount: {c.Instance.Amount} Id:{c.Instance.CorrelationId}"))
            //        .Publish(context => new OrderSuccessCreatedEvent() { name = "mokhtarsd" })
            //        .Finalize());

            SetCompletedWhenFinalized();
        }

        public State ProcessOrder { get; private set; }
        public Event<OrderCreatedEvent> OrderCreate { get; private set; }

        public Event<OrderFailedCreatedEvent> OrderFailed { get; private set; }

        public Event<OrderSuccessCreatedEvent> OrderSuccess { get; private set; }

    }
}
