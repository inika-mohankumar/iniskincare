using System.ComponentModel.DataAnnotations;

namespace iniskincare.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // like "bodycare", "facecare", etc.
        public string ImageUrl { get; set; }
        public int Price { get; set; }

        public string Ingredients { get; set; }
        public string Description { get; set; }
    }
}
