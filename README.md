# 🧾 InvoiceSystem

Sistema distribuido para la generación automática de facturas en PDF a partir de ventas enviadas a través de Azure Service Bus, con almacenamiento en Blob Storage.

---

## 🚀 Descripción

**InvoiceSystem** es un proyecto basado en arquitectura desacoplada que permite emitir facturas en PDF de forma automática, generarlas desde un API, 
procesarlas mediante una Azure Function, y almacenarlas en Azure Blob Storage.

Ideal como base para sistemas ERP simples, automatización de cobros o pruebas de integración con Azure Serverless.

---

## 🧱 Arquitectura

```text
[Invoice.API] ---> [Azure Service Bus Queue: facturacion] ---> [Invoice.Functions] ---> [Azure Blob Storage]
                          ↑                                                   ↓
                     VentaDto (JSON)                                  PDF generado con QuestPDF

🧰 Tecnologías
.NET 8

Azure Service Bus

Azure Functions

Azure Blob Storage

QuestPDF

GitHub Actions para CI/CD

Docker

Azure Container Apps para el deploy de Invoice.API

⚙️ Configuración
🔐 Variables de entorno / App Settings
En Azure debes configurar los siguientes settings (como Application Settings):

Nombre	Tipo	Valor ejemplo
ServiceBusConnectionString	Connection string	Endpoint=sb://...
AzureWebJobsStorage	Connection string	DefaultEndpointsProtocol=https;AccountName=...
BlobContainerName	App Setting	facturas

💡 Tanto AzureWebJobsStorage como BlobConnectionString pueden tener el mismo valor si usás una sola cuenta de almacenamiento.

📦 Deploy
Invoice.API (Azure Container App)
Este proyecto se deployea mediante GitHub Actions y se ejecuta como contenedor en Azure:

Build y push a GitHub Container Registry (GHCR)

Deploy automático a Azure Container App

Podés ver el workflow en: .github/workflows/deploy.yml

Invoice.Functions
Se deployea como Azure Function con trigger de Service Bus:

Usar func azure functionapp publish <nombre> o configurar pipeline en Actions.

🧪 Cómo probar
1- Llamá al endpoint de ventas:

POST /ventas
Content-Type: application/json

{
  "id": "123",
  "cliente": "Juan Pérez",
  "fecha": "2025-07-24T12:00:00",
  "productos": [
    { "nombre": "Notebook", "precioUnitario": 800000, "cantidad": 1 },
    { "nombre": "Mouse", "precioUnitario": 15000, "cantidad": 2 }
  ]
}

2- Verificá los logs en Azure Functions

Se debería generar un PDF

Se sube al contenedor Blob configurado (ej: facturas/factura-123.pdf)

📄 Licencia de QuestPDF
Este proyecto usa QuestPDF con licencia Community:

QuestPDF.Settings.License = LicenseType.Community;

Si tu organización factura más de USD 1M al año, debés adquirir una licencia comercial. Más info en: questpdf.com/license




