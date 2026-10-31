using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class PatchProductDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? ProductName { get; set; } //property is nullable

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockLevel { get; set; }
    }
}