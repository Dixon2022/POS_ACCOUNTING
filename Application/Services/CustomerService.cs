using Microsoft.EntityFrameworkCore;
using ERP_Software.Application.DTOs;
using ERP_Software.Application.Interfaces;
using ERP_Software.Domain.Entities;
using ERP_Software.Infrastructure.Data;

namespace ERP_Software.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ERPDbContext _context;

    public CustomerService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.Name)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<CustomerDto> CreateAsync(CustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            CreatedDate = DateTime.Now,
            IsActive = true
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        dto.Id = customer.Id;
        dto.CreatedDate = customer.CreatedDate;
        return dto;
    }

    public async Task UpdateAsync(CustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(dto.Id);
        if (customer != null)
        {
            customer.Name = dto.Name;
            customer.Phone = dto.Phone;
            customer.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleActiveAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            customer.IsActive = !customer.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    private static CustomerDto MapToDto(Customer customer) => new()
    {
        Id = customer.Id,
        Name = customer.Name,
        Phone = customer.Phone,
        CreatedDate = customer.CreatedDate,
        IsActive = customer.IsActive
    };
}

