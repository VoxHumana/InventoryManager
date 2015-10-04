using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public string Supplier { get; set; }
    }
}