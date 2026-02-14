using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

public interface IIncomeService
{
    Task<IEnumerable<IncomeDto>> GetAllAsync();
    Task<IEnumerable<IncomeDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IncomeDto?> GetByIdAsync(int id);
    Task<IncomeDto> CreateAsync(IncomeDto dto);
    Task UpdateAsync(IncomeDto dto);
    Task DeleteAsync(int id);
}
