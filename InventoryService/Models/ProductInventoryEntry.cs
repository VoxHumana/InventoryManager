namespace InventoryService.Models
{
    public class ProductInventoryEntry
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }
        public int Margin { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}