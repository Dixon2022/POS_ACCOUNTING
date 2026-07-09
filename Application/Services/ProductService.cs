using Microsoft.EntityFrameworkCore;
using ERP_Software.Application.DTOs;
using ERP_Software.Application.Interfaces;
using ERP_Software.Domain.Entities;
using ERP_Software.Infrastructure.Data;

namespace ERP_Software.Application.Services;

public class ProductService : IProductService
{
    private readonly SalonDbContext _context;

    public ProductService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Variants)
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetActiveAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Variants)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product != null ? MapToDto(product) : null;
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            SupplierId = dto.SupplierId,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        dto.Id = product.Id;
        return dto;
    }

    public async Task UpdateAsync(ProductDto dto)
    {
        var product = await _context.Products.FindAsync(dto.Id);
        if (product != null)
        {
            product.Name = dto.Name;
            product.SupplierId = dto.SupplierId;
            product.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleActiveAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        SupplierId = product.SupplierId,
        SupplierName = product.Supplier?.Name,
        IsActive = product.IsActive,
        VariantsCount = product.Variants?.Count ?? 0
    };
}
