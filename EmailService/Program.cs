using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailService
{
  internal static class Program
  {
    public static void Main()
    {
      var factory = new ConnectionFactory { HostName = "localhost" };
      using var connection = factory.CreateConnection();
      using var channel = connection.CreateModel();
      channel.ExchangeDeclare("topic_logs", "topic");
      var queueName = channel.QueueDeclare().QueueName;
      
      channel.QueueBind(queueName,"topic_logs", "booking.booked");

      Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

      var consumer = new EventingBasicConsumer(channel);
      consumer.Received += (model, ea) =>
      {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = ea.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'",
          routingKey,
          message);
      };
      channel.BasicConsume(queueName, true, consumer);

      Console.WriteLine(" Press [enter] to exit.");
      Console.ReadLine();
    }
  }
}