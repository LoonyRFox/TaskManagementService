using MediatR;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Commands.CreateLog;

public class CreateLogCommand : IRequest<BaseResult<long>>
{
    public Guid LogId { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}