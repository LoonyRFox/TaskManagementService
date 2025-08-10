using AutoMapper;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Domain.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagementService.Application.Profiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        // Маппинг из TaskItem в TaskDto
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt))
            .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreatedBy)); // Если UpdateBy нет в TaskItem — игнорируем
    }
}