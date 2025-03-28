namespace TestTask;

public interface IWeatherService
{
    string GetWeather(string city);
}

public class RealWeatherService : IWeatherService
{
    public string GetWeather(string city)
    {
        Console.WriteLine($"Запрос погоды для города {city} из реального сервиса...");
        return $"Погода в {city}: 25°C, солнечно";
    }
}

public class LoggingDecorator : IWeatherService
{
    private readonly IWeatherService _weatherService;

    public LoggingDecorator(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public string GetWeather(string city)
    {
        Console.WriteLine($"[Лог] Запрос погоды для города: {city}");
        var result = _weatherService.GetWeather(city);
        Console.WriteLine($"[Лог] Получен результат: {result}");
        return result;
    }
}

public class CachingDecorator : IWeatherService
{
    private readonly IWeatherService _weatherService;
    private readonly Dictionary<string, string> _cache = new();

    public CachingDecorator(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public string GetWeather(string city)
    {
        if (_cache.TryGetValue(city, out var weather))
        {
            Console.WriteLine($"[Кэш] Возвращаем данные из кэша для {city}");
            return weather;
        }

        var result = _weatherService.GetWeather(city);
        _cache[city] = result;
        return result;
    }
}

internal static class Program
{
    private static void Main()
    {
        IWeatherService weatherService = new RealWeatherService();
        
        weatherService = new CachingDecorator(weatherService);
        weatherService = new LoggingDecorator(weatherService);
        
        Console.WriteLine(weatherService.GetWeather("Москва"));
        
        Console.WriteLine(weatherService.GetWeather("Москва"));
    }
}