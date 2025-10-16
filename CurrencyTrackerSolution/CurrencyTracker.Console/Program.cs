using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Infrastructure.Extentions;
using CurrencyTracker.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var solutionRoot = FindSolutionRoot();

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(solutionRoot)
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddDbContextWithServices(configuration);
services.AddLogging(options =>
{
    options.AddConsole();
});
services.AddHttpClient();
services.AddSingleton<ICurrencyLoader, CrbCurrencyLoader>();

IServiceProvider provider = services.BuildServiceProvider();

while (true)
{
    using (var scope = provider.CreateScope())
    {
        try
        {
            ICurrencyLoader currencyLoader = scope.ServiceProvider.GetRequiredService<ICurrencyLoader>();

            int daystoLoad = int.Parse(configuration["LoadParameters:DaysToLoad"] ?? "10");

            await currencyLoader.LoadCurrenciesAndSaveAsync(daystoLoad);
        }
        catch (Exception ex)
        {
            ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex, $"Возникла ошибка при запросе курсов валют");

            break;
        }
    }

    int daysToNext = int.Parse(configuration["LoadParameters:Interval"] ?? "1");

    Thread.Sleep(TimeSpan.FromDays(daysToNext));
}

Console.ReadKey();

// Ищем файл конфигураций пока не найдем
static string FindSolutionRoot(string currentPath = null)
{
    currentPath ??= Directory.GetCurrentDirectory();
    var directory = new DirectoryInfo(currentPath);

    while (directory != null)
    {
        if (directory.GetFiles("*.sln").Any() || directory.GetFiles("appsettings.json").Any())
        {
            return directory.FullName;
        }
        directory = directory.Parent;
    }

    throw new DirectoryNotFoundException("Не удалось найти корень решения");
}