using TaskManagementServiceLoging.Application.Wrappers;
using TaskManagementServiceLoging.Domain;

namespace TaskManagementServiceLoging.Application.Interfaces;

public interface ILogIncomingMessageValidator
{
    BaseResult<OutboxMessage> Validate(string message);
}