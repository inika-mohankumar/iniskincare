using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iniskincare.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        // Optional (only needed if you display product name directly)
        [Required]
        public string ProductName { get; set; }

        // Optional: for displaying or tracking logged-in user's email
        public string? UserEmail { get; set; }
    }
}
