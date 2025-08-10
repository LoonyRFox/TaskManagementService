using Shouldly;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Features.Commands.CreateTask;
using TaskManagementService.Application.Features.Commands.UpdateTask;
using TaskManagementService.Application.Wrappers;
using TaskService.FunctionalTest.Common;

namespace TaskService.FunctionalTest.ApiEndpoints
{
    [Collection("TaskFunctionalTests")]
    public class TaskFunctionalTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client = factory.CreateClient();

        [Fact]
        public async Task GetPagedListTask_ShouldReturnPagedResponse()
        {
            // Arrange
            var url = ApiRoutes.V1.Task.GetPagedListTask;

            // Act
            var result = await client.GetAndDeserializeAsync<PagedResponse<TaskDto>>(url);

            // Assert
            result.Success.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnTask()
        {
            // Arrange
            var url = ApiRoutes.V1.Task.GetTaskById.AddQueryString("id", "1");

            // Act
            var result = await client.GetAndDeserializeAsync<BaseResult<TaskDto>>(url);

            // Assert
            result.Success.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Id.ShouldBe(1);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnTaskId()
        {
            // Arrange
            var url = ApiRoutes.V1.Task.CreateTask;
            var command = new CreateTaskCommand()
            {
                Title = RandomDataExtensionMethods.RandomString(10),
                Description = RandomDataExtensionMethods.RandomString(100),
            };
            var ghostAccount = await client.GetGhostAccount();

            // Act
            var result = await client.PostAndDeserializeAsync<BaseResult<long>>(url, command, ghostAccount.JwToken);

            // Assert
            result.Success.ShouldBeTrue();
            result.Data.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateTask_ShouldSucceed()
        {
            // Arrange
            var url = ApiRoutes.V1.Task.UpdateTask;
            var command = new UpdateTaskCommand()
            {
                Id = 1,
                Title = RandomDataExtensionMethods.RandomString(10),
                Description = RandomDataExtensionMethods.RandomString(100),
            };
            var ghostAccount = await client.GetGhostAccount();

            // Act
            var result = await client.PutAndDeserializeAsync<BaseResult>(url, command, ghostAccount.JwToken);

            // Assert
            result.Success.ShouldBeTrue();
        }

        [Fact]
        public async Task DeleteTask_ShouldSucceed()
        {
            // Arrange
            var url = ApiRoutes.V1.Task.DeleteTask.AddQueryString("id", "3");
            var ghostAccount = await client.GetGhostAccount();

            // Act
            var result = await client.DeleteAndDeserializeAsync<BaseResult>(url, ghostAccount.JwToken);

            // Assert
            result.Success.ShouldBeTrue();
        }

    }
}
