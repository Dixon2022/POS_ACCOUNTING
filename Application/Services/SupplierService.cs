using Microsoft.EntityFrameworkCore;
using ERP_Software.Application.DTOs;
using ERP_Software.Application.Interfaces;
using ERP_Software.Domain.Entities;
using ERP_Software.Infrastructure.Data;

namespace ERP_Software.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ERPDbContext _context;

    public SupplierService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        return await _context.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierDto>> GetActiveAsync()
    {
        return await _context.Suppliers
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        return supplier != null ? MapToDto(supplier) : null;
    }

    public async Task<SupplierDto> CreateAsync(SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            ContactPerson = dto.ContactPerson,
            CreatedDate = DateTime.Now,
            IsActive = true
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        dto.Id = supplier.Id;
        dto.CreatedDate = supplier.CreatedDate;
        return dto;
    }

    public async Task UpdateAsync(SupplierDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(dto.Id);
        if (supplier != null)
        {
            supplier.Name = dto.Name;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.ContactPerson = dto.ContactPerson;
            supplier.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleActiveAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            supplier.IsActive = !supplier.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    private static SupplierDto MapToDto(Supplier supplier) => new()
    {
        Id = supplier.Id,
        Name = supplier.Name,
        Phone = supplier.Phone,
        Email = supplier.Email,
        ContactPerson = supplier.ContactPerson,
        CreatedDate = supplier.CreatedDate,
        IsActive = supplier.IsActive
    };
}

