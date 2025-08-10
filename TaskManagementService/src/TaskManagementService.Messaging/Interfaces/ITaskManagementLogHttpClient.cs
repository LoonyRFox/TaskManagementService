using Refit;
using System.Runtime.InteropServices;
using TaskManagementService.Application.DTOs;

namespace TaskManagementService.Messaging.Interfaces;

public interface ITaskManagementLogHttpClient
{
    [Post("/api/v1/activity-log")]
    Task<IApiResponse> CreateAsync(CreateLogRequest request);
}