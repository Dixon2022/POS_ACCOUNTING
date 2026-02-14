using SilSalon_v._1.Application.DTOs;

namespace SilSalon_v._1.Application.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseDto>> GetAllAsync();
    Task<IEnumerable<PurchaseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<PurchaseDto>> GetPendingPaymentsAsync();
    Task<PurchaseDto?> GetByIdAsync(int id);
    Task<PurchaseDto> CreateAsync(PurchaseDto dto);
    Task<PurchaseDto?> AddPaymentAsync(int purchaseId, decimal amount);
    Task<PurchaseDto?> MarkAsReceivedAsync(int purchaseId);
    Task DeleteAsync(int id);
}
