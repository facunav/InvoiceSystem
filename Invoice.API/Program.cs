using Invoice.API.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();     
builder.Logging.SetMinimumLevel(LogLevel.Information); 

// Add services to the container.
builder.Services.Configure<ServiceBusConfig>(
    builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
