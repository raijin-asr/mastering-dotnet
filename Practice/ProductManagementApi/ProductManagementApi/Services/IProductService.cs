using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs;
using ProductManagementApi.Models;

namespace ProductManagementApi.Services;

public interface IProductService
{
    ProductResponseDto ToResponseDto(Product product);

    #region HTTP Methods
    Task<ProductListResponseDto> GetAllAsync(
    string? search, string? sortBy, bool descending, int page, int pageSize);

    Task<Product?> GetByIdAsync(int id);

    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> PatchAsync(int id, PatchProductDto dto);
    Task<bool> DeleteAsync(int id);

    #endregion

}