using TaskManagementService.Application.DTOs.Info;

namespace TaskManagementService.Application.Interfaces;

public interface IInfoService
{
    Task<HealthCheckResponse> HealthCheckAsync();
}