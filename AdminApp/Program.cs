using System;
using Autofac;
using RawRabbit.DependencyInjection.Autofac;
using RawRabbit.vNext;

namespace AdminApp
{
  internal static class Program
  {
    private static IContainer Container { get; set; }

    private static void Main(string[] args)
    {
      var builder = new ContainerBuilder();
      builder.RegisterRawRabbit("guest:guest@localhost:5672/");
      Container = builder.Build();

      var client = BusClientFactory.CreateDefault();
      client.SubscribeAsync<object>(async (msg, context) =>
      {
        Console.WriteLine($"Received Booking: {msg}.");
      }, cfg =>
        cfg.WithExchange(cfgExchange => cfgExchange.WithName("dlx_exchange"))
          .WithQueue(cfgQueue => cfgQueue.WithName("dead_letter_queue"))
          .WithRoutingKey("dlx_key")
        );
    }
  }
}