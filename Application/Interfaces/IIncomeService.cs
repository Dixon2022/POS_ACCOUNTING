using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface IIncomeService
{
    Task<IEnumerable<IncomeDto>> GetAllAsync();
    Task<IEnumerable<IncomeDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IncomeDto?> GetByIdAsync(int id);
    Task<IncomeDto> CreateAsync(IncomeDto dto);
    Task UpdateAsync(IncomeDto dto);
    Task DeleteAsync(int id);
}
