using Microsoft.EntityFrameworkCore;
using SilSalon_v._1.Application.DTOs;
using SilSalon_v._1.Application.Interfaces;
using SilSalon_v._1.Domain.Entities;
using SilSalon_v._1.Infrastructure.Data;

namespace SilSalon_v._1.Application.Services;

public class ServiceService : IServiceService
{
    private readonly SalonDbContext _context;

    public ServiceService(SalonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllAsync()
    {
        return await _context.Services
            .OrderBy(s => s.Name)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceDto>> GetActiveAsync()
    {
        return await _context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    public async Task<ServiceDto?> GetByIdAsync(int id)
    {
        var service = await _context.Services.FindAsync(id);
        return service != null ? MapToDto(service) : null;
    }

    public async Task<ServiceDto> CreateAsync(ServiceDto dto)
    {
        var service = new Service
        {
            Name = dto.Name,
            BasePrice = dto.BasePrice,
            IsActive = true
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        dto.Id = service.Id;
        return dto;
    }

    public async Task UpdateAsync(ServiceDto dto)
    {
        var service = await _context.Services.FindAsync(dto.Id);
        if (service != null)
        {
            service.Name = dto.Name;
            service.BasePrice = dto.BasePrice;
            service.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service != null)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleActiveAsync(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service != null)
        {
            service.IsActive = !service.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    private static ServiceDto MapToDto(Service service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        BasePrice = service.BasePrice,
        IsActive = service.IsActive
    };
}
