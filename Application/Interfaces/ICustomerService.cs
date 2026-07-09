using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<IEnumerable<CustomerDto>> GetActiveAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CustomerDto dto);
    Task UpdateAsync(CustomerDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
}
