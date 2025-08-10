using TaskManagementService.Application.Parameters;

namespace TaskManagementService.Application.DTOs.Account.Requests
{
    public class GetAllUsersRequest : PaginationRequestParameter
    {
        public string Name { get; set; }
    }
}
