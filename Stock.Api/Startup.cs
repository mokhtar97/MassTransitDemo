using MassTransit;
using MassTransit.Management;
using MassTransit.Management.Core;
using MassTransit.Management.Sagas.Order;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Stock.Api.Consumers;
using Stock.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock.Api", Version = "v1" });
            });
            services.AddDbContext<StockContext>(options =>
            options.UseSqlServer(Configuration["ConnectionString"]));


            // var z = new StockConsumer();
            //   var y = new StockOrderFailed();
            // var w= new OrderSuccess();
             var r= new OrderSubmitConsumer();
            
            List<ConsumerBaseEntity> consumers = new List<ConsumerBaseEntity>();
           // consumers.Add(new ConsumerBaseEntity() { ConsumerType = z.GetType(), InstanceId = "OrderCreated" });
           // consumers.Add(new ConsumerBaseEntity() { ConsumerType = y.GetType(), InstanceId = "OrderFailed" });
           //consumers.Add(new ConsumerBaseEntity() { ConsumerType = w.GetType(), InstanceId = "OrderSuccess" });
           consumers.Add(new ConsumerBaseEntity() { ConsumerType = r.GetType(), InstanceId = "OrderSubmitted" });
            services.AddMassTransitConfiguration(consumers);         

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
