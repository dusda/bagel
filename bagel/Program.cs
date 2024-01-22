using bagel;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
  .AddScoped<IBagelService, InMemoryBagelService>()
  .AddLogging();

var app = builder.Build();
var provider = app.Services;

app.MapGet("/", () => "Hello World!");

app.MapGet("/bagels", async (IBagelService bagelService, ILogger<Program> logger) =>
{
  logger.LogInformation("Getting bagels");
  var bagels = await bagelService.GetBagelsAsync();
  return Results.Ok(bagels);
});

app.MapGet("/bagels/{id}", async (IBagelService bagelService, ILogger<Program> logger, int id) =>
{
  logger.LogInformation("Getting bagel {Id}", id);
  var bagel = await bagelService.GetBagelAsync(id);
  return Results.Ok(bagel);
});

app.MapGet("/bagels/bomb", async (IBagelService bagelService, ILogger<Program> logger) =>
{
  try
  {
    logger.LogInformation("Getting bagel {Id}", 42);
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
