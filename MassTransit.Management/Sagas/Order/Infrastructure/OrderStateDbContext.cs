
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Sagas.Order.Infrastructure
{
    public class OrderStateDbContext : SagaDbContext
    {
        public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options)
            : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }
        //  public DbSet<OrderStateMap> OrderStateMaps { get; set; } }
    }

    public class OrderStateDbContextDesignFactory : IDesignTimeDbContextFactory<OrderStateDbContext>
    {
        public OrderStateDbContext CreateDbContext(string[] args)
        {

            //var optionsBuilder = new DbContextOptionsBuilder<StatisticsContext>().UseSqlServer("Server=AHMED-BAGHDADI\\SQLEXPRESS;Initial Catalog=GAEBStatistics;Integrated Security=true" + Environment.NewLine);

            var optionsBuilder = new DbContextOptionsBuilder<OrderStateDbContext>().UseSqlServer("Server=.; Initial Catalog=GaebSaga-Order; Trusted_Connection = True;");

            return new OrderStateDbContext(optionsBuilder.Options);

        }
    }

}
