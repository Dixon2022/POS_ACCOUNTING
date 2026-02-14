using Microsoft.EntityFrameworkCore;
using SilSalon_v._1.Application.DTOs;
using SilSalon_v._1.Application.Interfaces;
using SilSalon_v._1.Domain.Entities;
using SilSalon_v._1.Infrastructure.Data;

namespace SilSalon_v._1.Application.Services;

public class ProductVariantService : IProductVariantService
{
    private readonly SalonDbContext _context;

    public ProductVariantService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductVariantDto>> GetAllAsync()
    {
        return await _context.ProductVariants
            .Include(pv => pv.Product)
            .OrderBy(pv => pv.Product.Name)
            .ThenBy(pv => pv.Name)
            .Select(pv => MapToDto(pv))
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductVariantDto>> GetActiveAsync()
    {
        return await _context.ProductVariants
            .Include(pv => pv.Product)
            .Where(pv => pv.IsActive && pv.Product.IsActive)
            .OrderBy(pv => pv.Product.Name)
            .ThenBy(pv => pv.Name)
            .Select(pv => MapToDto(pv))
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductVariantDto>> GetByProductIdAsync(int productId)
    {
        return await _context.ProductVariants
            .Include(pv => pv.Product)
            .Where(pv => pv.ProductId == productId)
            .OrderBy(pv => pv.Name)
            .Select(pv => MapToDto(pv))
            .ToListAsync();
    }

    public async Task<ProductVariantDto?> GetByIdAsync(int id)
    {
        var variant = await _context.ProductVariants
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(pv => pv.Id == id);
        return variant != null ? MapToDto(variant) : null;
    }

    public async Task<ProductVariantDto> CreateAsync(ProductVariantDto dto)
    {
        var variant = new ProductVariant
        {
            ProductId = dto.ProductId,
            Name = dto.Name,
            Code = dto.Code,
            CostPrice = dto.CostPrice,
            SalePrice = dto.SalePrice,
            Stock = dto.Stock,
            IsActive = true
        };

        _context.ProductVariants.Add(variant);
        await _context.SaveChangesAsync();

        dto.Id = variant.Id;
        return dto;
    }

    public async Task UpdateAsync(ProductVariantDto dto)
    {
        var variant = await _context.ProductVariants.FindAsync(dto.Id);
        if (variant != null)
        {
            variant.ProductId = dto.ProductId;
            variant.Name = dto.Name;
            variant.Code = dto.Code;
            variant.CostPrice = dto.CostPrice;
            variant.SalePrice = dto.SalePrice;
            variant.Stock = dto.Stock;
            variant.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var variant = await _context.ProductVariants.FindAsync(id);
        if (variant != null)
        {
            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleActiveAsync(int id)
    {
        var variant = await _context.ProductVariants.FindAsync(id);
        if (variant != null)
        {
            variant.IsActive = !variant.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateStockAsync(int id, int quantity)
    {
        var variant = await _context.ProductVariants.FindAsync(id);
        if (variant != null)
        {
            variant.Stock += quantity;
            if (variant.Stock < 0) variant.Stock = 0;
            await _context.SaveChangesAsync();
        }
    }

    private static ProductVariantDto MapToDto(ProductVariant variant) => new()
    {
        Id = variant.Id,
        ProductId = variant.ProductId,
        ProductName = variant.Product?.Name,
        Name = variant.Name,
        Code = variant.Code,
        CostPrice = variant.CostPrice,
        SalePrice = variant.SalePrice,
        Stock = variant.Stock,
        IsActive = variant.IsActive
    };
}
