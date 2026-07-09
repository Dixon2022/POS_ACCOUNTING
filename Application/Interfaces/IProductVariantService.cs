using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface IProductVariantService
{
    Task<IEnumerable<ProductVariantDto>> GetAllAsync();
    Task<IEnumerable<ProductVariantDto>> GetActiveAsync();
    Task<IEnumerable<ProductVariantDto>> GetByProductIdAsync(int productId);
    Task<ProductVariantDto?> GetByIdAsync(int id);
    Task<ProductVariantDto> CreateAsync(ProductVariantDto dto);
    Task UpdateAsync(ProductVariantDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
    Task UpdateStockAsync(int id, int quantity);
}
