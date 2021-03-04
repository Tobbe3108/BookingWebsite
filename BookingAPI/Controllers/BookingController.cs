using System;
using System.Threading.Tasks;
using BookingAPI.Producers;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;

namespace BookingAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class BookingController : ControllerBase
  {
    private readonly BookingProducer _bookingProducer;

    public BookingController(BookingProducer bookingProducer)
    {
      _bookingProducer = bookingProducer;
    }

    [HttpPost]
    [Route("Book")]
    public async Task<ActionResult> PostBookingRequest(BookingRequest bookingRequest)
    {
      await _bookingProducer.PublishBookingRequest(bookingRequest);
      return Ok(bookingRequest);
    }
    
    [HttpPost]
    [Route("Cancel/{bookingId}")]
    public async Task<ActionResult> PostCancelBookingRequest(Guid? bookingId)
    {
      await _bookingProducer.PublishCancelBookingRequest(new CancelBookingRequest{BookingId = bookingId});
      return Ok(bookingId);
    }
  }
}