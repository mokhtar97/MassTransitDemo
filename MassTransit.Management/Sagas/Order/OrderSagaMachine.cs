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
            InstanceState(s => s.CurrentState, SubmitOrderState);


            #region Declare Events
            Event(() => OrderCreate, x =>
              x.CorrelateBy(order => order.OrderId.ToString(),
              context => context.Message.OrderId.ToString())
              .SelectId(context => NewId.NextGuid()));


            //From Stock Service
            Event(() => OrderSubmittedFailedFromStock, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => OrderSubmittedSuccessFullyFromStock, x => x.CorrelateById(context =>
               context.Message.CorrelationId));



            //From Shippment Service
            //Event(() => OrderShippedSuccessfully, x => x.CorrelateById(context =>
            //    context.Message.CorrelationId));

            //Event(() => OrderShippedFailed, x => x.CorrelateById(context =>
            //    context.Message.CorrelationId));

            #endregion

            #region Initial State
            Initially(
                  When(OrderCreate)
                     .Then(context =>
                     {
                         context.Instance.OrderId = context.Data.OrderId;
                         context.Instance.Name = context.Data.Name;
                         context.Instance.Amount = context.Data.Amount;
                     })
                     .Publish(context => new OrderSubmitCreatedEvent() { CorrelationId = context.Instance.CorrelationId, Amount = context.Instance.Amount })
                      .TransitionTo(SubmitOrderState)
                      );
            #endregion

            #region During
            During(SubmitOrderState,
                 When(OrderSubmittedFailedFromStock)
                    .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Stock" ,Id=context.Instance.OrderId}),
                 When(OrderSubmittedSuccessFullyFromStock)
                 .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Stock", Id = context.Instance.OrderId }),



                    .Finalize());




            //During(ShippmentOrderState,
                
            //    When(OrderShippedSuccessfully)          
            //     .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Shippment",Id=context.Instance.OrderId }),
            //     When(OrderShippedFailed)
            //    .Then(context => context.Instance.OrderFailedCreatedDate = DateTime.Now)
            //        .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Shippment", Id = context.Instance.OrderId })
            //        .Finalize());
            #endregion

            SetCompletedWhenFinalized();
        }

        public State SubmitOrderState { get; private set; }

        public State ShippmentOrderState { get; private set; }
        public Event<OrderCreatedEvent> OrderCreate { get; private set; }


        //From Stock Service
        public Event<OrderSubmittedFailedEvent> OrderSubmittedFailedFromStock { get; private set; }

        public Event<OrderSubmittedSuccessfullyEvent> OrderSubmittedSuccessFullyFromStock { get; private set; }


        //From Shippment Service
        public Event<OrderShippedSuccessfullyEvent> OrderShippedSuccessfully { get; private set; }

        public Event<OrderShippedFailedEvent> OrderShippedFailed { get; private set; }
    }
}
