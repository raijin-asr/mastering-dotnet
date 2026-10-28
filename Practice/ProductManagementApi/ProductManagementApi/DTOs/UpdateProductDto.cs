using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ProductName { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockLevel { get; set; }
    }
}
