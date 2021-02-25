using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookingAPI.Models.Requests;
using RabbitMQ.Client;

namespace BookingAPI.Producers
{
  public class BookingProducer
  {
    private readonly IModel? _channel;

    public BookingProducer()
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      var connection = factory.CreateConnection();
      _channel = connection.CreateModel();
      _channel.ExchangeDeclare("topic_logs","topic");
    }
    public async Task PublishBookingRequest(BookingRequest bookingRequest)
    {
      using var channel = _channel;
      _channel.BasicPublish("topic_logs",
        "booking.booked",
        null,
        Encoding.UTF8.GetBytes(JsonSerializer.Serialize(bookingRequest)));
    }

    public async Task PublishCancelBookingRequest(CancelBookingRequest cancelBookingRequest)
    {
      using var channel = _channel;
      _channel.BasicPublish("topic_logs",
        "booking.cancelled",
        null,
        Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cancelBookingRequest)));
    }
  }
}