using System.ComponentModel.DataAnnotations;

namespace PracticeSandBox.Models
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters")]
        public string ProductName { get; set; }

        [Range(0.01, 100000.00, ErrorMessage ="Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Stock cannot be negative.")]
        public int StockLevel { get; set; }

        public string? SuccessMessage { get; set; }
    }
}
