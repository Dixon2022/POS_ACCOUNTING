using Microsoft.EntityFrameworkCore;
using ERP_Software.Application.DTOs;
using ERP_Software.Application.Interfaces;
using ERP_Software.Domain.Enums;
using ERP_Software.Infrastructure.Data;

namespace ERP_Software.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ERPDbContext _context;

    public DashboardService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        // Ingresos de hoy (ventas + otros ingresos)
        var salesToday = await _context.Sales
            .Where(s => s.Date.Date == today)
            .SumAsync(s => s.Total);

        var otherIncomeToday = await _context.Incomes
            .Where(i => i.Date.Date == today)
            .SumAsync(i => i.Amount);

        var incomeToday = salesToday + otherIncomeToday;

        // Ingresos del mes
        var salesMonth = await _context.Sales
            .Where(s => s.Date.Date >= startOfMonth && s.Date.Date <= endOfMonth)
            .SumAsync(s => s.Total);

        var otherIncomeMonth = await _context.Incomes
            .Where(i => i.Date.Date >= startOfMonth && i.Date.Date <= endOfMonth)
            .SumAsync(i => i.Amount);

        var incomeMonth = salesMonth + otherIncomeMonth;

        // Gastos del mes
        var expensesMonth = await _context.Expenses
            .Where(e => e.Date.Date >= startOfMonth && e.Date.Date <= endOfMonth)
            .SumAsync(e => e.Amount);

        // Compras del mes
        var purchasesMonth = await _context.Purchases
            .Where(p => p.Date.Date >= startOfMonth && p.Date.Date <= endOfMonth)
            .SumAsync(p => p.Total);

        // Ganancia neta
        var netProfit = incomeMonth - expensesMonth - purchasesMonth;

        // Clientes nuevos este mes
        var newCustomersMonth = await _context.Customers
            .Where(c => c.CreatedDate.Date >= startOfMonth && c.CreatedDate.Date <= endOfMonth)
            .CountAsync();

        // Clientes atendidos este mes (únicos en ventas)
        var customersServedMonth = await _context.Sales
            .Where(s => s.Date.Date >= startOfMonth && s.Date.Date <= endOfMonth && s.CustomerId.HasValue)
            .Select(s => s.CustomerId)
            .Distinct()
            .CountAsync();

        // Producto más vendido
        var bestSellingProduct = await _context.SaleItems
            .Where(si => si.ItemType == ItemType.Product && si.ProductVariantId.HasValue)
            .Where(si => si.Sale.Date.Date >= startOfMonth && si.Sale.Date.Date <= endOfMonth)
            .GroupBy(si => si.ProductVariantId)
            .Select(g => new { ProductVariantId = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefaultAsync();

        string? bestProductName = null;
        if (bestSellingProduct?.ProductVariantId != null)
        {
            var variant = await _context.ProductVariants
                .Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.Id == bestSellingProduct.ProductVariantId);
            bestProductName = variant != null ? $"{variant.Product.Name} - {variant.Name}" : null;
        }

        // Servicio más solicitado
        var mostRequestedService = await _context.SaleItems
            .Where(si => si.ItemType == ItemType.Service && si.ServiceId.HasValue)
            .Where(si => si.Sale.Date.Date >= startOfMonth && si.Sale.Date.Date <= endOfMonth)
            .GroupBy(si => si.ServiceId)
            .Select(g => new { ServiceId = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefaultAsync();

        string? mostServiceName = null;
        if (mostRequestedService?.ServiceId != null)
        {
            var service = await _context.Services.FindAsync(mostRequestedService.ServiceId);
            mostServiceName = service?.Name;
        }

        // Proveedor principal (más compras)
        var mainSupplier = (await _context.Purchases
            .Where(p => p.Date.Date >= startOfMonth && p.Date.Date <= endOfMonth)
            .GroupBy(p => p.SupplierId)
            .Select(g => new { SupplierId = g.Key, TotalAmount = g.Sum(x => x.Total) })
            .ToListAsync())
            .OrderByDescending(x => x.TotalAmount)
            .FirstOrDefault();

        string? mainSupplierName = null;
        if (mainSupplier != null)
        {
            var supplier = await _context.Suppliers.FindAsync(mainSupplier.SupplierId);
            mainSupplierName = supplier?.Name;
        }

        // Valor total del inventario
        var totalInventoryValue = await _context.ProductVariants
            .Where(pv => pv.IsActive)
            .SumAsync(pv => pv.Stock * pv.CostPrice);

        // Ventas últimos 7 días
        var salesLast7Days = new List<ChartDataPoint>();
        for (int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            var dailySales = await _context.Sales
                .Where(s => s.Date.Date == date)
                .SumAsync(s => s.Total);

            salesLast7Days.Add(new ChartDataPoint
            {
                Label = date.ToString("dd/MM"),
                Value = dailySales
            });
        }

        // Gastos vs Ingresos del mes (por semana)
        var expensesVsIncomeMonth = new List<ChartDataPoint>
        {
            new() { Label = "Ingresos", Value = incomeMonth },
            new() { Label = "Gastos", Value = expensesMonth },
            new() { Label = "Compras", Value = purchasesMonth }
        };

        return new DashboardDto
        {
            IncomeToday = incomeToday,
            IncomeMonth = incomeMonth,
            ExpensesMonth = expensesMonth,
            PurchasesMonth = purchasesMonth,
            NetProfit = netProfit,
            NewCustomersMonth = newCustomersMonth,
            CustomersServedMonth = customersServedMonth,
            BestSellingProduct = bestProductName,
            MostRequestedService = mostServiceName,
            MainSupplier = mainSupplierName,
            TotalInventoryValue = totalInventoryValue,
            SalesLast7Days = salesLast7Days,
            ExpensesVsIncomeMonth = expensesVsIncomeMonth
        };
    }
}

