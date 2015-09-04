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
                Quantity = quantity;
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("ERROR: Invalid arguments to construct ProductInventoryEntry");
            }
        }
        public int Quantity { get; set; }

        public int Margin
        {
            get
            {
                return (int) ((Price - Cost)/Price);
            }
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double Profit {
            get
            {
                return Price - Cost;
            } 
        }
    }
}