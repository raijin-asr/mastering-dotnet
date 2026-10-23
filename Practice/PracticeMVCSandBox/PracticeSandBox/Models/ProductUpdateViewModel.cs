using System.ComponentModel.DataAnnotations;

namespace PracticeSandBox.Models
{
    public class ProductUpdateViewModel
    {
        [Required(ErrorMessage = "Product name is required!")]
        [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters")]
        public string ProductName { get; set; }

        [Range(0.01, 1000.00, ErrorMessage ="Price must be between Rs. 0.01 to Rs. 1000.00")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "Stock must be between 0 and 100")]
        public int StockLevel { get; set; }

        public string? SuccessMessage { get; set; }
    }
}
