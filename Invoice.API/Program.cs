using Invoice.API.Configs;

var builder = WebApplication.CreateBuilder(args);

// Configuración de logging
builder.Logging.ClearProviders(); // Limpia cualquier proveedor por defecto
builder.Logging.AddConsole();     // Agrega logging a consola (visible en Azure log stream)
builder.Logging.SetMinimumLevel(LogLevel.Information); // Cambiá a Debug si querés más detalle

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
