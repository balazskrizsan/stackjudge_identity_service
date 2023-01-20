namespace WeatherService;

public static class WeatherForecastApi
{
    public static void MapWeatherForecastApi(this WebApplication app)
    {
        app.MapGet("/weatherforecast", GetWeatherForecast);
    }

    private static WeatherForecast[] GetWeatherForecast()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast =
            from index in Enumerable.Range(1, 5)
            select new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            );

        return forecast.ToArray();
    }
}

