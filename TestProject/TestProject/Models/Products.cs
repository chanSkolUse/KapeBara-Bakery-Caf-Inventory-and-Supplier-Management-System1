// Models/Product.cs - Updated
using System.Drawing;

namespace TestProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public byte[] ProductImage { get; set; }  // Store image as byte array
        public string ImagePath { get; set; }     // Or store image path
    }
}