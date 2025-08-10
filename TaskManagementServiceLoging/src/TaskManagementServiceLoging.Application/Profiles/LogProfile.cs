using AutoMapper;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Domain.Entity;

namespace TaskManagementServiceLoging.Application.Profiles;

public class LogProfile : Profile
{
    public LogProfile()
    {
        // Маппинг из LogModel в LogDto
        CreateMap<LogModel, LogDto>();

    }
}