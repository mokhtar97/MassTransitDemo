
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Sagas.Order.Infrastructure
{
    public class OrderStateMap : SagaClassMap<OrderSagaStatus>
    {
        protected override void Configure(EntityTypeBuilder<OrderSagaStatus> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.OrderId);
            entity.Property(x => x.Name);
            entity.Property(x => x.OrderCreatedDate);
            entity.Property(x => x.OrderFailedCreatedDate);
            entity.Property(x => x.Amount);

            // If using Optimistic concurrency, otherwise remove this property
            //  entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
