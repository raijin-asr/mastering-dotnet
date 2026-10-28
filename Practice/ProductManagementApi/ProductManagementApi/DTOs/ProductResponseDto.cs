namespace ProductManagementApi.DTOs
{
    public class ProductResponseDto
    {
        //no validation attributes. It's currently designed to return data, not validate incoming requests
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockLevel { get; set; }
    }
}
