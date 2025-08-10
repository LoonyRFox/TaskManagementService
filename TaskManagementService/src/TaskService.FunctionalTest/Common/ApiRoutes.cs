namespace TaskService.FunctionalTest.Common
{
    internal static class ApiRoutes
    {
        internal static class V1
        {
            internal static class Account
            {
                internal const string Authenticate = "/api/v1/Account/Authenticate";
                internal const string ChangeUserName = "/api/v1/Account/ChangeUserName";
                internal const string ChangePassword = "/api/v1/Account/ChangePassword";
                internal const string Start = "/api/v1/Account/Start";
            }

            internal static class Task
            {
                internal const string GetPagedListTask = "/api/v1/Task/GetPagedListTask";
                internal const string GetTaskById = "/api/v1/Task/GetTaskById";
                internal const string CreateTask = "/api/v1/Task/CreateTask";
                internal const string UpdateTask = "/api/v1/Task/UpdateTask";
                internal const string DeleteTask = "/api/v1/Task/DeleteTask";
            }
        }
        internal static string AddQueryString(this string url, string key, string value)
        {
            var separator = url.Contains("?") ? "&" : "?";
            return $"{url}{separator}{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";
        }
    }
}
