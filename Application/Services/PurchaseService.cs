using Microsoft.EntityFrameworkCore;
using SilSalon_v._1.Application.DTOs;
using SilSalon_v._1.Application.Interfaces;
using SilSalon_v._1.Domain.Entities;
using SilSalon_v._1.Infrastructure.Data;

namespace SilSalon_v._1.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly SalonDbContext _context;

    public PurchaseService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseDto>> GetAllAsync()
    {
        return await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv.Product)
            .OrderByDescending(p => p.Date)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<IEnumerable<PurchaseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv.Product)
            .Where(p => p.Date.Date >= startDate.Date && p.Date.Date <= endDate.Date)
            .OrderByDescending(p => p.Date)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<PurchaseDto?> GetByIdAsync(int id)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv.Product)
            .FirstOrDefaultAsync(p => p.Id == id);
        return purchase != null ? MapToDto(purchase) : null;
    }

    public async Task<PurchaseDto> CreateAsync(PurchaseDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var purchase = new Purchase
            {
                Date = dto.Date,
                SupplierId = dto.SupplierId,
                Total = dto.Items.Sum(i => i.Total)
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            foreach (var itemDto in dto.Items)
            {
                var purchaseItem = new PurchaseItem
                {
                    PurchaseId = purchase.Id,
                    ProductVariantId = itemDto.ProductVariantId,
                    Quantity = itemDto.Quantity,
                    UnitCost = itemDto.UnitCost,
                    Total = itemDto.Total
                };

                _context.PurchaseItems.Add(purchaseItem);

                // Incrementar stock
                var variant = await _context.ProductVariants.FindAsync(itemDto.ProductVariantId);
                if (variant != null)
                {
                    variant.Stock += itemDto.Quantity;
                    // Actualizar precio de costo si cambió
                    if (itemDto.UnitCost > 0)
                    {
                        variant.CostPrice = itemDto.UnitCost;
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            dto.Id = purchase.Id;
            return dto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (purchase != null)
        {
            // Revertir stock
            foreach (var item in purchase.Items)
            {
                var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
                if (variant != null)
                {
                    variant.Stock -= item.Quantity;
                    if (variant.Stock < 0) variant.Stock = 0;
                }
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }

    private static PurchaseDto MapToDto(Purchase purchase) => new()
    {
        Id = purchase.Id,
        Date = purchase.Date,
        SupplierId = purchase.SupplierId,
        SupplierName = purchase.Supplier?.Name,
        Total = purchase.Total,
        Items = purchase.Items.Select(i => new PurchaseItemDto
        {
            Id = i.Id,
            PurchaseId = i.PurchaseId,
            ProductVariantId = i.ProductVariantId,
            ProductVariantName = i.ProductVariant != null 
                ? $"{i.ProductVariant.Product?.Name} - {i.ProductVariant.Name}" 
                : null,
            Quantity = i.Quantity,
            UnitCost = i.UnitCost,
            Total = i.Total
        }).ToList()
    };
}
