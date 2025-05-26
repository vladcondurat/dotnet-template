using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Application.Use_Classes.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace SmartRealEstateManagement.IntegrationTests
{
    public class UsersControllerTests : IntegrationTestBase
    {
        public UsersControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedUserId()
        {
            ResetDatabase();

            var command = new CreateUserCommand
            {
                Username = "TestUsername",
            };

            var response = await Client.PostAsJsonAsync("/api/v1/Users", command);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Guid>(responseString);
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            ResetDatabase();

            var response = await Client.GetAsync("/api/v1/Users");

            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            Assert.NotNull(users);
            Assert.IsType<List<UserDto>>(users);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            ResetDatabase();

            var createCommand = new CreateUserCommand
            {
                Username = "TestUsername",
            };
            var createResponse = await Client.PostAsJsonAsync("/api/v1/Users", createCommand);
            createResponse.EnsureSuccessStatusCode();
            var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());

            var response = await Client.GetAsync($"/api/v1/Users/{createdId}");

            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal(createdId, user.Id);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenUserIsUpdated()
        {
            ResetDatabase();

            var createCommand = new CreateUserCommand
            {
                Username = "TestUsername",
            };
            var createResponse = await Client.PostAsJsonAsync("/api/v1/Users", createCommand);
            createResponse.EnsureSuccessStatusCode();
            var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());

            var updateCommand = new UpdateUserCommand
            {
                Id = createdId,
                Username = "UpdatedUsername",
            };

            var response = await Client.PutAsJsonAsync($"/api/v1/Users/{createdId}", updateCommand);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            ResetDatabase();

            var createCommand = new CreateUserCommand
            {
                Username = "TestUsername",
            };
            var createResponse = await Client.PostAsJsonAsync("/api/v1/Users", createCommand);
            createResponse.EnsureSuccessStatusCode();
            var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());

            var response = await Client.DeleteAsync($"/api/v1/Users/{createdId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
