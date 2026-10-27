using Microsoft.AspNetCore.Mvc;

namespace ProductManagementApi.Controllers
{
    [ApiController] //enables API-specific behavior,
    [Route("[controller]")] //defines the route pattern. Here, [controller] is replaced with WeatherForecast, so the route becomes /WeatherForecast
    public class WeatherForecastController : ControllerBase //inherits from ControllerBase instead of MVC's Controller, which provides API-oriented functionality, such as returning status codes and response bodies,
    {
        private static readonly string[] Summaries = new[] //Array
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //Dependency Injection
        private readonly ILogger<WeatherForecastController> _logger; //record application events and errors.

        // ILogger injected to constructor
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get() //returns sequence of WeatherForeCast objects, response is serialized data
        {
            //properties are populated using an object initializer
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray(); //materializes the generated sequence into an array containing five forecast objects
        } //The action then returns that array, and ASP.NET Core writes the JSON response
    }
}
