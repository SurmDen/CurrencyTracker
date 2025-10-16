using CurrencyTracker.Infrastructure.Extentions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;

// ��������� ����� appsettings.json �� ����� �������
builder.Configuration
    .SetBasePath(solutionRoot)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

// ��������� ���������� IServiceCollection
builder.Services.AddDbContextWithServices(builder.Configuration);
builder.Services.AddCustomAuthenticationWithServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromHours(2);
});
builder.Services.AddLogging();
builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Currency Tracker",
        Description = "API ��� ������ � ������� ����� �� ��",
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

app.UseSession();

app.UseRouting();

app.UseStaticFiles();

// ���� ������������ ������������� - ����� ����� ����������� � ��������� �� ������ 
app.Use(async (context, next) =>
{
    string? jwtToken = context.Session.GetString("token");

    if (!string.IsNullOrEmpty(jwtToken))
    {
        context.Request.Headers.Append("Authorization", $"Bearer {jwtToken}");
    }

    await next();
});

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
