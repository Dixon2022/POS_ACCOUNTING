using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetAllAsync();
    Task<IEnumerable<ServiceDto>> GetActiveAsync();
    Task<ServiceDto?> GetByIdAsync(int id);
    Task<ServiceDto> CreateAsync(ServiceDto dto);
    Task UpdateAsync(ServiceDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
}
