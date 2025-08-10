using AutoMapper;
using TaskManagementService.Application.DTOs;
using TaskManagementService.Domain.Messages;

namespace TaskManagementService.Application.Profiles;

public class OutboxMappingProfile : Profile
{
    public OutboxMappingProfile()
    {
        CreateMap<OutboxMessage, CreateLogRequest>();
    }
}