using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RateLimitingWithResponseCache.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 1000, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IEnumerable<WeatherForecast> Get()
        {  

            Console.WriteLine(  );
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                RandomSecond=DateTime.Now.Second

            })
            .ToArray();
        }

        [HttpPost("/AddStudent")]
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string CacheKey = $"#Student/{student.Id}";
            if (CacheHelper.KeyExists(CacheKey))
            {
                return BadRequest("Student With same data Already Exists");
            }
            CacheHelper.StringSet(CacheKey, JsonConvert.SerializeObject(student));
            return Created("", student.Id);


        }

        [HttpGet("/GetStudentById/{id}")]
        public IActionResult GetStudentById(int id)
        {
            string data = "";
            Student student = null;
            string CacheKey = $"#Student/{id}";
            if (!CacheHelper.KeyExists(CacheKey))
            {
                return NotFound();
            }
            else
            {
                data = CacheHelper.StringGet(CacheKey);
                student =JsonConvert.DeserializeObject<Student>(data);
                return Ok(student); 
            }
        }
    }
}
