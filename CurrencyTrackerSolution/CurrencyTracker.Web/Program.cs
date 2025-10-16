using CurrencyTracker.Infrastructure.Extentions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;

// Добавляем общий appsettings.json из корня решения
builder.Configuration
    .SetBasePath(solutionRoot)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

// Кастомные расширения IServiceCollection
builder.Services.AddDbContextWithServices(builder.Configuration);
builder.Services.AddCustomAuthenticationWithServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddLogging();
builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Currency Tracker",
        Description = "API для работы с курсами валют ЦБ РФ",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
