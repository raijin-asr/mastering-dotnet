using ProductManagementApi.DTOs;
using ProductManagementApi.Models;

namespace ProductManagementApi.Services;

public interface IProductService
{
    ProductResponseDto ToResponseDto(Product product);
    Task<ProductListResponseDto> GetAllAsync(
    string? search, string? sortBy, bool descending, int page, int pageSize);

    Task<Product?> GetByIdAsync(int id);
}