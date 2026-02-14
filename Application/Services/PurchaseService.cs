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

    public async Task<IEnumerable<PurchaseDto>> GetPendingPaymentsAsync()
    {
        return await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv.Product)
            .Where(p => p.PendingBalance > 0 || !p.ProductReceived)
            .OrderByDescending(p => p.Date)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<PurchaseDto> CreateAsync(PurchaseDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Calcular saldo pendiente
            var total = dto.Items.Sum(i => i.Total);
            var pendingBalance = total - dto.DepositAmount;
            if (pendingBalance < 0) pendingBalance = 0;

            var purchase = new Purchase
            {
                Date = dto.Date,
                SupplierId = dto.SupplierId,
                Total = total,
                DepositAmount = dto.DepositAmount,
                PendingBalance = pendingBalance,
                ProductReceived = dto.ProductReceived,
                PaymentMethod = dto.PaymentMethod,
                Notes = dto.Notes
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

                // Incrementar stock solo si el producto fue recibido
                if (dto.ProductReceived)
                {
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
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            dto.Id = purchase.Id;
            dto.PendingBalance = pendingBalance;
            return dto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PurchaseDto?> AddPaymentAsync(int purchaseId, decimal amount)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(pv => pv.Product)
            .FirstOrDefaultAsync(p => p.Id == purchaseId);

        if (purchase == null) return null;

        // Agregar el abono al monto ya pagado
        purchase.DepositAmount += amount;
        
        // Recalcular saldo pendiente
        purchase.PendingBalance = purchase.Total - purchase.DepositAmount;
        if (purchase.PendingBalance < 0) purchase.PendingBalance = 0;

        await _context.SaveChangesAsync();
        return MapToDto(purchase);
    }

    public async Task<PurchaseDto?> MarkAsReceivedAsync(int purchaseId)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
                .ThenInclude(i => i.ProductVariant)
            .FirstOrDefaultAsync(p => p.Id == purchaseId);

        if (purchase == null || purchase.ProductReceived) return null;

        // Marcar como recibido
        purchase.ProductReceived = true;

        // Agregar al inventario ahora que se recibe
        foreach (var item in purchase.Items)
        {
            var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
            if (variant != null)
            {
                variant.Stock += item.Quantity;
                // Actualizar precio de costo
                if (item.UnitCost > 0)
                {
                    variant.CostPrice = item.UnitCost;
                }
            }
        }

        await _context.SaveChangesAsync();
        return MapToDto(purchase);
    }

    public async Task DeleteAsync(int id)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (purchase != null)
        {
            // Revertir stock solo si el producto fue recibido
            if (purchase.ProductReceived)
            {
                foreach (var item in purchase.Items)
                {
                    var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
                    if (variant != null)
                    {
                        variant.Stock -= item.Quantity;
                        if (variant.Stock < 0) variant.Stock = 0;
                    }
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
        DepositAmount = purchase.DepositAmount,
        PendingBalance = purchase.PendingBalance,
        ProductReceived = purchase.ProductReceived,
        PaymentMethod = purchase.PaymentMethod,
        Notes = purchase.Notes,
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
