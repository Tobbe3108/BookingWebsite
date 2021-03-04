using System;

namespace Models.Requests
{
  public class CancelBookingRequest
  {
    public Guid? BookingId { get; set; }
  }
}