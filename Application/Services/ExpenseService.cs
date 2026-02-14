using Microsoft.EntityFrameworkCore;
using SilSalon_v._1.Application.DTOs;
using SilSalon_v._1.Application.Interfaces;
using SilSalon_v._1.Domain.Entities;
using SilSalon_v._1.Infrastructure.Data;

namespace SilSalon_v._1.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly SalonDbContext _context;

    public ExpenseService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Date)
            .Select(e => MapToDto(e))
            .ToListAsync();
    }

    public async Task<IEnumerable<ExpenseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Expenses
            .Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
            .OrderByDescending(e => e.Date)
            .Select(e => MapToDto(e))
            .ToListAsync();
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        return expense != null ? MapToDto(expense) : null;
    }

    public async Task<ExpenseDto> CreateAsync(ExpenseDto dto)
    {
        var expense = new Expense
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            Category = dto.Category,
            PaymentMethod = dto.PaymentMethod,
            Notes = dto.Notes
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        dto.Id = expense.Id;
        return dto;
    }

    public async Task UpdateAsync(ExpenseDto dto)
    {
        var expense = await _context.Expenses.FindAsync(dto.Id);
        if (expense != null)
        {
            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Category = dto.Category;
            expense.PaymentMethod = dto.PaymentMethod;
            expense.Notes = dto.Notes;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Expenses
            .Where(e => !string.IsNullOrEmpty(e.Category))
            .Select(e => e.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    private static ExpenseDto MapToDto(Expense expense) => new()
    {
        Id = expense.Id,
        Description = expense.Description,
        Amount = expense.Amount,
        Date = expense.Date,
        Category = expense.Category,
        PaymentMethod = expense.PaymentMethod,
        Notes = expense.Notes
    };
}
