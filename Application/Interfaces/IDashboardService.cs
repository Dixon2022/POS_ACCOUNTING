using ERP_Software.Application.DTOs;

namespace ERP_Software.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}
