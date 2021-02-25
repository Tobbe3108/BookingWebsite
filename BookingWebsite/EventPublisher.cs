using MassTransit;

namespace BookingWebsite
{
  public class EventPublisher
  {
    private readonly IPublishEndpoint _publishEndpoint;

    // public EventPublisher(IPublishEndpoint publishEndpoint)
    // {
    //   _publishEndpoint = publishEndpoint;
    // }
    
    public void PublishMessage()
    {
      //_publishEndpoint.Publish("Test");
    }
  }
}