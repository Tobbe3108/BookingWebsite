using System;

namespace BookingAPI.Models.Requests
{
  public class CancelBookingRequest
  {
    public Guid? BookingId { get; set; }
  }
}