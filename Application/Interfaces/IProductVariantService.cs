using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

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
