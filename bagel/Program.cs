using System.Diagnostics.Metrics;
using bagel;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;
var name = builder.Environment.ApplicationName;

config.AddEnvironmentVariables();

var bagelMeter = new Meter("bagel_requests_total");
var bagelCounter = bagelMeter.CreateCounter<long>(
  "bagel_requests_total",
  "Counts bagels.");

services
  .AddScoped<IBagelService, InMemoryBagelService>()
  .AddLogging()
  .AddOpenTelemetry()
  .ConfigureResource(_ => _.AddService(name))
  .WithMetrics(_ => _
    .AddAspNetCoreInstrumentation()
    .AddMeter(bagelMeter.Name)
    .AddMeter("Microsoft.AspnetCore.Hosting")
    .AddMeter("Microsoft.AspnetCore.Server.Kestrel")
    .AddPrometheusExporter())
  .WithTracing(_ => _
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddSource(name)
    .AddOtlpExporter(_ => _
      .Endpoint = new Uri(config["Otlp:Endpoint"]!)));

var app = builder.Build();
var provider = app.Services;

app.MapPrometheusScrapingEndpoint();
app.MapGet("/", () => "Hello World!");

app.MapGet("/bagels", async (IBagelService bagelService, ILogger<Program> logger) =>
{
  logger.LogInformation("Getting bagels");
  bagelCounter.Add(1);
  var bagels = await bagelService.GetBagelsAsync();
  return Results.Ok(bagels);
});

app.MapGet("/bagels/{id}", async (IBagelService bagelService, ILogger<Program> logger, int id) =>
{
  logger.LogInformation("Getting bagel {Id}", id);
  bagelCounter.Add(1);
  var bagel = await bagelService.GetBagelAsync(id);
  return Results.Ok(bagel);
});

app.MapGet("/bagels/bomb", async (IBagelService bagelService, ILogger<Program> logger) =>
{
  try
  {
    logger.LogInformation("Getting bagel {Id}", 42);
    bagelCounter.Add(1);
    var bagel = await bagelService.GetBagelAsync(42);
    return Results.Ok(bagel);
  }
  catch (KeyNotFoundException exc)
  {
    logger.LogError(exc, "Bagel not found");
    return Results.NotFound();
  }
});

app.Run();
