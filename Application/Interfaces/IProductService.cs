using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<IEnumerable<ProductDto>> GetActiveAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductDto dto);
    Task UpdateAsync(ProductDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
    Task CleanupDeletedProductsAsync();
}
