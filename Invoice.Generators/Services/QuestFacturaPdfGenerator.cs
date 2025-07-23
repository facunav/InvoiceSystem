using Invoice.Generators.Interfaces;
using Invoice.Shared.Models;
using QuestPDF.Fluent;

public class QuestFacturaPdfGenerator : IFacturaPdfGenerator
{
    public byte[] GenerarFactura(VentaDto venta)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text($"Factura #{venta.Id}").FontSize(20).Bold();
                page.Content().Column(col =>
                {
                    col.Item().Text($"Cliente: {venta.Cliente}");
                    col.Item().Text($"Email: {venta.Email}");
                    col.Item().Text($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(70);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Producto").Bold();
                            header.Cell().Text("Cant.").Bold();
                            header.Cell().Text("Precio").Bold();
                        });

                        foreach (var item in venta.Items)
                        {
                            table.Cell().Text(item.Producto);
                            table.Cell().Text(item.Cantidad.ToString());
                            table.Cell().Text($"${item.Precio:N2}");
                        }
                    });

                    var total = venta.Items.Sum(i => i.Cantidad * i.Precio);
                    col.Item().AlignRight().Text($"Total: ${total:N2}").FontSize(14).Bold();
                });
            });
        });

        return document.GeneratePdf();
    }
}
