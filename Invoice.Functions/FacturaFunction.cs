using Azure.Storage.Blobs;
using Invoice.Generators.Interfaces;
using Invoice.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Invoice.Functions
{
    public class FacturaFunction
    {
        private readonly ILogger _logger;
        private readonly IFacturaPdfGenerator _pdfGenerator;
        private readonly string _blobConnectionString;
        private readonly string _blobContainerName;

        public FacturaFunction(ILoggerFactory loggerFactory, IConfiguration configuration, IFacturaPdfGenerator pdfGenerator)
        {
            _logger = loggerFactory.CreateLogger<FacturaFunction>();
            _blobContainerName = configuration["BlobContainerName"];
            _pdfGenerator = pdfGenerator;
            _blobConnectionString = configuration["BlobConnectionString"];
        }

        [Function("FacturaFunction")]
        public async Task Run(
            [ServiceBusTrigger("facturacion", Connection = "ServiceBusConnectionString")]
            string message)
        {
            var venta = JsonSerializer.Deserialize<VentaDto>(message);

            if (venta is null)
            {
                _logger.LogWarning("No se pudo deserializar la venta.");
                return;
            }

            try 
            {
                var pdfBytes = _pdfGenerator.GenerarFactura(venta);

                var blobClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);
                await blobClient.CreateIfNotExistsAsync();

                var blobName = $"factura-{venta.Id}.pdf";
                var blob = blobClient.GetBlobClient(blobName);

                using var stream = new MemoryStream(pdfBytes);
                await blob.UploadAsync(stream, overwrite: true);

                _logger.LogInformation($"🧾 Factura generada y guardada como {blobName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir la factura al blob storage");
            }
        }
    }
}
