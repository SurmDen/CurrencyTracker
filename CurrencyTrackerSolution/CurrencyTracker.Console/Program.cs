using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Infrastructure.Extentions;
using CurrencyTracker.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName)
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