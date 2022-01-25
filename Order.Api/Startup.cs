using MassTransit.Management;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Order.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;
using MassTransit;
using Order.Api.Consumers;
using MassTransit.Management.Core;
using MassTransit.Management.Sagas.Order;
using MassTransit.Saga;

namespace Order.Api
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

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order.Api", Version = "v1" });
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDbContext<OrderContext>(options =>
             options.UseSqlServer(Configuration["ConnectionString"]));
      
            
          
            //var z = new OrderFailedConsumer();
            List<ConsumerBaseEntity> consumers = new List<ConsumerBaseEntity>();
            //consumers.Add(new ConsumerBaseEntity() { ConsumerType = z.GetType(), InstanceId = "OrderFailed" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order.Api v1"));
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
