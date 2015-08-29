using System;

namespace InventoryManager
{
    public class ProductInventoryEntry
    {
        public double Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        public ProductInventoryEntry() { }
        public ProductInventoryEntry(double quantity, string name, double price, double cost)
        {
            this.Quantity = quantity;
            this.Name = name;
            this.Price = price;
            this.Cost = cost;
        }
        public ProductInventoryEntry(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException("product");
            this.Name = product.Name;
            this.Price = product.Price;
            this.Cost = product.Cost;
            Quantity = quantity;
        }
    }
}
