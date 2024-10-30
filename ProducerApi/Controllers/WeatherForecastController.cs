using MassTransit;
using Messaging;
using Microsoft.AspNetCore.Mvc;

namespace ProducerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private MassTransitOutboxContext _outboxContext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPublishEndpoint publishEndpoint, MassTransitOutboxContext outboxContext)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _outboxContext = outboxContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string? name = null, CancellationToken cancellationToken = default)
        {
            var id = Guid.NewGuid();

            await _publishEndpoint.Publish<SomethingHappened>(new SomethingHappened
            {
                Id = id,
                Name = name ?? string.Empty,
            }, cancellationToken);

            await _publishEndpoint.Publish<SomethingElseHappened>(new SomethingElseHappened
            {
                Id = id,
                Name = name ?? string.Empty,
            }, cancellationToken);

            await _outboxContext.SaveChangesAsync(cancellationToken);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
