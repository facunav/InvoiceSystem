using Azure.Storage.Blobs;
using Invoice.Generators;
using Invoice.Generators.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddScoped<IFacturaPdfGenerator, QuestFacturaPdfGenerator>();

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var blobConnectionString = config["AzureWebJobsStorage"];
            return new BlobServiceClient(blobConnectionString);
        });
    })
    .Build();

host.Run();
