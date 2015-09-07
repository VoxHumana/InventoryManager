using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int Profit { get; set; }
        public int Margin { get; set; }
        public string ImageFilePath { get; set; }
    }
}