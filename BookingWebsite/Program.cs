using System;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BookingWebsite
{
  public static class Program
  {
    public static async Task Main(string[] args)
    {
      await CreateHostBuilder(args).Build().RunAsync();
    }

    private static WebAssemblyHostBuilder CreateHostBuilder(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault();
      builder.ConfigureContainer(new AutofacServiceProviderFactory());
      builder.RootComponents.Add<App>("#app");
      builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
      builder.RegisterType<EventPublisher>();
      return builder;
    }
    
    private static ContainerBuilder Register(this ContainerBuilder builder)
    {
      return builder;
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
          x.AddConsumer<EventConsumer>();
          
          x.UsingInMemory((context, cfg) =>
          {
            cfg.TransportConcurrencyLimit = 100;

            cfg.ConfigureEndpoints(context);
          });
        });

      services.AddMassTransitHostedService();
    }
  }
}