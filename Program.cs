using ExchangeRateServer.Helpers;
using ExchangeRateServer.Interfaces;
using ExchangeRateServer.Logging;
using ExchangeRateServer.Services;
using ExchangeRateServer.Services.RequestStrategyBTC;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

builder.Services.AddSingleton<IHttpClient, ExchangeRateServer.Helpers.HttpClient>();
builder.Services.AddSingleton<IBTCRateRequest, BTCRateRequestCoinApiService>();
builder.Services.AddSingleton<IRateRequestService, RateRequestService>();
builder.Services.AddSingleton<ICacheService, LocalFileCacheService>();

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");
IConfiguration configuration = configurationBuilder.Build();

if (!File.Exists(configuration.GetValue<string>("LocalCache")))
    File.WriteAllText(configuration.GetValue<string>("LocalCache"), "[]");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation($"Server running at Time: {DateTime.Now}");

app.Run();