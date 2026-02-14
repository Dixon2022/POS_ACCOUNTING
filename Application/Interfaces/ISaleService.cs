using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

public interface ISaleService
{
    Task<IEnumerable<SaleDto>> GetAllAsync();
    Task<IEnumerable<SaleDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SaleDto>> GetByDateRangeAndCustomerAsync(DateTime startDate, DateTime endDate, int? customerId);
    Task<IEnumerable<SaleDto>> GetPendingPaymentsAsync();
    Task<SaleDto?> GetByIdAsync(int id);
    Task<SaleDto> CreateAsync(SaleDto dto);
    Task<SaleDto?> AddPaymentAsync(int saleId, decimal amount);
    Task<SaleDto?> MarkAsDeliveredAsync(int saleId);
    Task DeleteAsync(int id);
}
