using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseDto>> GetAllAsync();
    Task<IEnumerable<ExpenseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<ExpenseDto> CreateAsync(ExpenseDto dto);
    Task UpdateAsync(ExpenseDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<string>> GetCategoriesAsync();
}
