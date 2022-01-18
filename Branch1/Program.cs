using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Branch1
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("Configuring web host ({Branch1})...", "Subscriber");
                var host = BuildWebHost(configuration, args);

                Log.Information("Starting web host ({Branch1})...", "Subscriber");
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({Branch1})!", "Subscriber");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
   WebHost.CreateDefaultBuilder(args)
   .CaptureStartupErrors(false)
   .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
   .UseStartup<Startup>()
   .UseContentRoot(Directory.GetCurrentDirectory()).UseSerilog()
   .Build();

        private static IConfiguration GetConfiguration()
        {
            string appsetting;

            appsetting = "appsettings.Development.json";

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile(appsetting, optional: false, reloadOnChange: true)
             .AddEnvironmentVariables();
            return builder.Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            return new LoggerConfiguration()
             .MinimumLevel.Verbose()
             //.Enrich.WithProperty("service1", "service1")
             //.Enrich.FromLogContext()
             //.WriteTo.File("service1.log.txt", rollingInterval: RollingInterval.Day)
             .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
             .ReadFrom.Configuration(configuration)
             .CreateLogger();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
