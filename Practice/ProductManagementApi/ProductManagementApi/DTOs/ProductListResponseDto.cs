
namespace ProductManagementApi.DTOs
{
    public class ProductListResponseDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        // Here use ProductResponseDto not Product coz API to return response DTOs, not database entities.
        public List<ProductResponseDto> Items { get; set; } = new();
    }
}
