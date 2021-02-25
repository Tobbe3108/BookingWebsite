using System;

namespace BookingAPI.Models
{
  public class Booking
  {
    public Guid? BookingId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
  }
}