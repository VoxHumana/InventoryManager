using System;
using System.Diagnostics;

namespace InventoryManager
{
    public class ProductInventoryEntry
    {
        public ProductInventoryEntry()
        {
        }

        public ProductInventoryEntry(double quantity, string name, double price, double cost)
        {
            Quantity = quantity;
            Name = name;
            Price = price;
            Cost = cost;
        }

        public ProductInventoryEntry(Product product, int quantity)
        {
            if (product == null) try
            {
                throw new ArgumentNullException("product");
            }
            catch (ArgumentNullException argumentNullException)
            {
                Debug.WriteLine("ERROR: Invalid arguments to construct ProductInventoryEntry");
            }
            Name = product.Name;
            Price = product.Price;
            Cost = product.Cost;
            Quantity = quantity;
        }

        public double Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
    }
}