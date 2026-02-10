namespace Catalog.API.Models
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Color { get; set; } = default!;
        public string Size { get; set; } = default!;

        public decimal Price { get; set; } 
        public int Stock { get; set; }
      
    }
}
