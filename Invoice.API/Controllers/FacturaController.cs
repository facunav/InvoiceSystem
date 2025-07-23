using Azure.Core;
using Azure.Messaging.ServiceBus;
using Invoice.API.Configs;
using Invoice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Invoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly ServiceBusConfig _config;

        public FacturaController(IOptions<ServiceBusConfig> config)
        {
            _config = config.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VentaDto venta)
        {
            try
            {
                var connectionString = _config.ServiceBusConnectionString;
                var queueName = _config.QueueName;

                await using var client = new ServiceBusClient(connectionString);
                var sender = client.CreateSender(queueName);

                var jsonBody = JsonSerializer.Serialize(venta);
                var message = new ServiceBusMessage(jsonBody);

                await sender.SendMessageAsync(message);

                return Ok("Mensaje enviado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error interno: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Ocurrió un error al generar la factura.");
            }
        }
    }
}
