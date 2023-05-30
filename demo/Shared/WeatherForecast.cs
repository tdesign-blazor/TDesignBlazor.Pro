using System.ComponentModel.DataAnnotations;

namespace TDesign.Admin.Demo.Shared
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        [Required]
        public string? Summary { get; set; } = string.Empty;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);


        public static IEnumerable<WeatherForecast> Gets()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Summary = "Hello",
                TemperatureC = new Random().Next(20, 40)
            })
            ;
        }
    }
}