using Microsoft.EntityFrameworkCore;
using SilSalon_v._1.Application.DTOs;
using SilSalon_v._1.Application.Interfaces;
using SilSalon_v._1.Domain.Entities;
using SilSalon_v._1.Domain.Enums;
using SilSalon_v._1.Infrastructure.Data;

namespace SilSalon_v._1.Application.Services;

public class SaleService : ISaleService
{
    private readonly SalonDbContext _context;

    public SaleService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv!.Product)
            .Include(s => s.Items)
                .ThenInclude(i => i.Service)
            .OrderByDescending(s => s.Date)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<SaleDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv!.Product)
            .Include(s => s.Items)
                .ThenInclude(i => i.Service)
            .Where(s => s.Date.Date >= startDate.Date && s.Date.Date <= endDate.Date)
            .OrderByDescending(s => s.Date)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<SaleDto>> GetByDateRangeAndCustomerAsync(DateTime startDate, DateTime endDate, int? customerId)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv!.Product)
            .Include(s => s.Items)
                .ThenInclude(i => i.Service)
            .Where(s => s.Date.Date >= startDate.Date && s.Date.Date <= endDate.Date);

        if (customerId.HasValue)
        {
            query = query.Where(s => s.CustomerId == customerId.Value);
        }

        return await query
            .OrderByDescending(s => s.Date)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<SaleDto?> GetByIdAsync(int id)
    {
        var sale = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv!.Product)
            .Include(s => s.Items)
                .ThenInclude(i => i.Service)
            .FirstOrDefaultAsync(s => s.Id == id);
        return sale != null ? MapToDto(sale) : null;
    }

    public async Task<SaleDto> CreateAsync(SaleDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var sale = new Sale
            {
                Date = dto.Date,
                CustomerId = dto.CustomerId,
                Total = dto.Items.Sum(i => i.Total),
                PaymentMethod = dto.PaymentMethod,
                Notes = dto.Notes
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var itemDto in dto.Items)
            {
                var saleItem = new SaleItem
                {
                    SaleId = sale.Id,
                    ItemType = itemDto.ItemType,
                    ProductVariantId = itemDto.ProductVariantId,
                    ServiceId = itemDto.ServiceId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Total = itemDto.Total
                };

                _context.SaleItems.Add(saleItem);

                // Disminuir stock si es producto
                if (itemDto.ItemType == ItemType.Product && itemDto.ProductVariantId.HasValue)
                {
                    var variant = await _context.ProductVariants.FindAsync(itemDto.ProductVariantId.Value);
                    if (variant != null)
                    {
                        variant.Stock -= itemDto.Quantity;
                        if (variant.Stock < 0) variant.Stock = 0;
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            dto.Id = sale.Id;
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
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale != null)
        {
            // Restaurar stock de productos
            foreach (var item in sale.Items.Where(i => i.ItemType == ItemType.Product && i.ProductVariantId.HasValue))
            {
                var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId!.Value);
                if (variant != null)
                {
                    variant.Stock += item.Quantity;
                }
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }

    private static SaleDto MapToDto(Sale sale) => new()
    {
        Id = sale.Id,
        Date = sale.Date,
        CustomerId = sale.CustomerId,
        CustomerName = sale.Customer?.Name,
        Total = sale.Total,
        PaymentMethod = sale.PaymentMethod,
        Notes = sale.Notes,
        Items = sale.Items.Select(i => new SaleItemDto
        {
            Id = i.Id,
            SaleId = i.SaleId,
            ItemType = i.ItemType,
            ProductVariantId = i.ProductVariantId,
            ProductVariantName = i.ProductVariant != null 
                ? $"{i.ProductVariant.Product?.Name} - {i.ProductVariant.Name}" 
                : null,
            ServiceId = i.ServiceId,
            ServiceName = i.Service?.Name,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Total = i.Total
        }).ToList()
    };
}
