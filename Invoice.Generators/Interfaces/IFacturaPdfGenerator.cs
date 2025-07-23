using Invoice.Shared.Models;

namespace Invoice.Generators.Interfaces
{
    public interface IFacturaPdfGenerator
    {
        byte[] GenerarFactura(VentaDto venta);
    }
}
