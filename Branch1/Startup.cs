using Branch1.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes; // for call to Immediate
using System.Data;
using MassTransit.Messages.Models.Commands;
using MassTransit.Management;

namespace Branch1
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Branch1", Version = "v1" });
            });
            //services.AddMassTransit(x =>
            //{
            //    //x.AddConsumer<DecisionConsumer>();

            //    x.SetKebabCaseEndpointNameFormatter();

            //    x.AddConsumer<DecisionConsumer>(
            //        c =>
            //        {
            //            c.UseMessageRetry(r =>
            //           {
            //               r.Interval(2,10000);
            //                // r.Handle<DataException>(x => x.Message.Contains("SQL"));
            //            });
            //            c.UseInMemoryOutbox();
            //        }
            //        )
            //    .Endpoint(c => c.InstanceId = "Branch1");
               
            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.ConfigureEndpoints(context);
            //    });

            //});
            //services.AddMassTransitHostedService();
            var x = new DecisionConsumer();
            services.AddMassTransitConsumerServices<Decision>(x, "mokh");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Branch1 v1"));
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
