using TaskManagementService.Application.DTOs;

namespace TaskManagementService.Application.Interfaces;

public interface ITranslator
{
    string this[string name]
    {
        get;
    }

    string GetString(string name);
    string GetString(TranslatorMessageDto input);
}