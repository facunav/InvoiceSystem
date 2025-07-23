namespace Invoice.Shared.Models
{
    public class VentaDto
    {
        public string Id { get; set; }    
        public string Cliente { get; set; } 
        public string Email { get; set; }  
        public DateTime Fecha { get; set; } 
        public List<ItemDto> Items { get; set; } 
        public decimal Total => Items?.Sum(i => i.Precio * i.Cantidad) ?? 0;
    }
}
