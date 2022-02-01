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
            InstanceState(s => s.CurrentState, SubmitOrderState, ShippmentOrderState);


            #region Declare Events

            Event(() => OrderCreate, x => x.CorrelateById(context => context.Message.OrderId));

            Event(() => OrderSubmitCreated, x => x.CorrelateById(context =>
               context.Message.OrderId));
            
            //From Stock Service
            Event(() => OrderSubmittedFailedFromStock, x => x.CorrelateById(context =>
                context.Message.OrderId));

            Event(() => OrderSubmittedSuccessFullyFromStock, x => x.CorrelateById(context =>
               context.Message.OrderId));


            //From Shippment Service
            Event(() => OrderShippedSuccessfully, x => x.CorrelateById(context =>
                context.Message.OrderId));

            Event(() => OrderShippedFailed, x => x.CorrelateById(context =>
                context.Message.OrderId));

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
                     .Publish(context => new OrderSubmitCreatedEvent() { CorrelationId = context.Instance.OrderId, Amount = context.Instance.Amount, Name = context.Instance.Name, OrderId = context.Instance.OrderId }),

                   When(OrderSubmittedFailedFromStock).Then(context => {
                       context.Instance.OrderCreatedDate = DateTime.Now;
                       context.Instance.OrderId = context.Data.OrderId;
                       Console.Out.WriteLineAsync(
                            $"Order Submitted processed. Id: Failed From Stock");
                   })
                    .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Stock", Id = context.Instance.OrderId }),
                  
                   
                    When(OrderSubmittedSuccessFullyFromStock)
                    .Then(context => {
                        context.Instance.OrderCreatedDate = DateTime.Now;
                        context.Instance.OrderId = context.Data.OrderId;
                    })
                    .ThenAsync(
                        context => Console.Out.WriteLineAsync(
                            $"Order Submitted processed. Id: {context.Instance.OrderId}"))

                    .Publish(context => new OrderShippmentStartEvent() { Name = " Start Shippment", OrderId = context.Instance.OrderId }).TransitionTo(ShippmentOrderState)                          
                      ) ;
            #endregion

            #region During Config
            #region SubmitStock
            //#region During
            //During(SubmitOrderState,
            //  Ignore(OrderShippedFailed),
            //     When(OrderSubmittedFailedFromStock)
            //     .Then(context => {
            //         context.Instance.OrderCreatedDate = DateTime.Now;
            //         context.Instance.OrderId = context.Data.OrderId;
            //        Console.Out.WriteLineAsync(
            //             $"Order Submitted processed. Id: Failed From Stock");
            //     })
            //        .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Stock" ,Id=context.Instance.OrderId}).Finalize(),
            //     When(OrderSubmittedSuccessFullyFromStock)
            //    .Then(context => {
            //        context.Instance.OrderCreatedDate = DateTime.Now;
            //        context.Instance.OrderId = context.Data.OrderId;
            //    })
            //        .ThenAsync(
            //            context => Console.Out.WriteLineAsync(
            //                $"Order Submitted processed. Id: {context.Instance.OrderId}"))

            //        .Publish(context => new OrderShippmentStartEvent() { Name = " Start Shippment" , OrderId = context.Instance.OrderId }).TransitionTo(ShippmentOrderState)

            //        ); 
            #endregion

            During(ShippmentOrderState,
                 Ignore(OrderSubmittedSuccessFullyFromStock),
                   When(OrderShippedSuccessfully)
                   .Then(context =>
                   {
                       context.Instance.OrderCreatedDate = DateTime.Now;
                       context.Instance.OrderId = context.Data.OrderId;
                   })
                    .Publish(context => new OrderFailedCreatedEvent() { Name = " Success  From Shippment", Id = context.Instance.OrderId }),
                 When(OrderShippedFailed)
                .Then(context =>
                {
                    context.Instance.OrderCreatedDate = DateTime.Now;
                    context.Instance.OrderId = context.Data.OrderId;
                })
                    .Publish(context => new OrderFailedCreatedEvent() { Name = " Failed  From Shippment", Id = context.Instance.OrderId })
                    .Finalize()
                    );
            #endregion


             SetCompletedWhenFinalized();
        }

        public State SubmitOrderState { get; private set; }

        public State ShippmentOrderState { get; private set; }
        public Event<OrderCreatedEvent> OrderCreate { get; private set; }

        public Event<OrderSubmitCreatedEvent> OrderSubmitCreated { get; private set; }

        
        //From Stock Service
        public Event<OrderSubmittedFailedEvent> OrderSubmittedFailedFromStock { get; private set; }

        public Event<OrderSubmittedSuccessfullyEvent> OrderSubmittedSuccessFullyFromStock { get; private set; }


        //From Shippment Service
        public Event<OrderShippedSuccessfullyEvent> OrderShippedSuccessfully { get; private set; }

        public Event<OrderShippedFailedEvent> OrderShippedFailed { get; private set; }
    }
}
