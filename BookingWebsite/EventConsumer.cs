using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BookingWebsite
{
  public class EventConsumer : IConsumer<string>
  {
    private readonly ILogger<EventConsumer> _logger;

    public EventConsumer(ILogger<EventConsumer> logger)
    {
      _logger = logger;
    }

    public async Task Consume(ConsumeContext<string> context)
    {
      _logger.LogInformation("Value: {Value}", context.Message);
    }
  }
}