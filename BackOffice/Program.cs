using System;
using Autofac;
using RawRabbit.DependencyInjection.Autofac;
using RawRabbit.vNext;

namespace BackOffice
{
  internal static class Program
  {
    private static IContainer Container { get; set; }
    public static void Main()
    {
      var builder = new ContainerBuilder();
      builder.RegisterRawRabbit("guest:guest@localhost:5672/");
      Container = builder.Build();
      
      var client = BusClientFactory.CreateDefault();
      client.SubscribeAsync<object>(async (msg, context) =>
      {
        Console.WriteLine($"Received: {msg}.");
      }, cfg => 
        cfg.WithExchange(e => e
            .WithName("booking_exchange"))
          .WithQueue(q => q
            .WithName("booking")
            .WithArgument("x-dead-letter-exchange", "dlx_exchange")
            .WithArgument("x-dead-letter-routing-key", "dlx_key")
            .WithArgument("x-max-length", 1))
          .WithRoutingKey("booking"));
    }
  }
}