using TaskManagementService.Application.DTOs;

namespace TaskManagementService.Application.Helpers;

public static class TranslatorMessages
{
    public static class AccountMessages
    {
        public static TranslatorMessageDto Account_NotFound_with_UserName(string userName) => new(nameof(Account_NotFound_with_UserName), [userName]);
        public static TranslatorMessageDto Username_is_already_taken(string userName) => new(nameof(Username_is_already_taken), [userName]);
        public static string Invalid_password() => nameof(Invalid_password);
    }
    public static class TaskMessages
    {
        public static TranslatorMessageDto Task_NotFound_with_id(long id)
            => new(nameof(Task_NotFound_with_id), [id.ToString()]);
    }
}