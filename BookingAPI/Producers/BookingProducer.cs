using System;
using System.Threading.Tasks;
using Models.Requests;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace BookingAPI.Producers
{
  public class BookingProducer
  {
    private readonly IBusClient _client;

    public BookingProducer()
    {      
      _client = BusClientFactory.CreateDefault();
    }
    public async Task PublishBookingRequest(BookingRequest bookingRequest)
    {     
      await _client.PublishAsync(bookingRequest, 
        Guid.NewGuid(), 
        cfg => cfg
          .WithExchange(cfgExchange => cfgExchange.WithName("booking_exchange"))
          .WithRoutingKey("booking.booked"));
    }

    public async Task PublishCancelBookingRequest(CancelBookingRequest cancelBookingRequest)
    {
      await _client.PublishAsync(cancelBookingRequest, 
        Guid.NewGuid(), 
        cfg => cfg
          .WithExchange(e => e
            .WithName("booking_exchange"))
          .WithRoutingKey("booking.cancel"));
    }
  }
}