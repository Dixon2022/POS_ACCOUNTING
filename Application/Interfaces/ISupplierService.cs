using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<IEnumerable<SupplierDto>> GetActiveAsync();
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<SupplierDto> CreateAsync(SupplierDto dto);
    Task UpdateAsync(SupplierDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
}
