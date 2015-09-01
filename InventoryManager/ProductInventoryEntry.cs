using System;
using System.Diagnostics;

namespace InventoryManager
{
    public class ProductInventoryEntry
    {
        public ProductInventoryEntry() { }

        public ProductInventoryEntry(Product product, int quantity)
        {
            try
            {
                Name = product.Name;
                Price = product.Price;
                Cost = product.Cost;
                Margin = product.Margin;
                Quantity = quantity;
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("ERROR: Invalid arguments to construct ProductInventoryEntry");
            }
        }
        public int Quantity { get; set; }
        public int Margin { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
    }
}