using Microsoft.EntityFrameworkCore;
using ERP_Software.Application.DTOs;
using ERP_Software.Application.Interfaces;
using ERP_Software.Domain.Entities;
using ERP_Software.Infrastructure.Data;

namespace ERP_Software.Application.Services;

public class IncomeService : IIncomeService
{
    private readonly ERPDbContext _context;

    public IncomeService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IncomeDto>> GetAllAsync()
    {
        return await _context.Incomes
            .OrderByDescending(i => i.Date)
            .Select(i => MapToDto(i))
            .ToListAsync();
    }

    public async Task<IEnumerable<IncomeDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Incomes
            .Where(i => i.Date.Date >= startDate.Date && i.Date.Date <= endDate.Date)
            .OrderByDescending(i => i.Date)
            .Select(i => MapToDto(i))
            .ToListAsync();
    }

    public async Task<IncomeDto?> GetByIdAsync(int id)
    {
        var income = await _context.Incomes.FindAsync(id);
        return income != null ? MapToDto(income) : null;
    }

    public async Task<IncomeDto> CreateAsync(IncomeDto dto)
    {
        var income = new Income
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date
        };

        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();

        dto.Id = income.Id;
        return dto;
    }

    public async Task UpdateAsync(IncomeDto dto)
    {
        var income = await _context.Incomes.FindAsync(dto.Id);
        if (income != null)
        {
            income.Description = dto.Description;
            income.Amount = dto.Amount;
            income.Date = dto.Date;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var income = await _context.Incomes.FindAsync(id);
        if (income != null)
        {
            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
        }
    }

    private static IncomeDto MapToDto(Income income) => new()
    {
        Id = income.Id,
        Description = income.Description,
        Amount = income.Amount,
        Date = income.Date
    };
}

