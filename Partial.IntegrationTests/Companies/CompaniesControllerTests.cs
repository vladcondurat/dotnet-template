// using System.Net;
// using System.Net.Http.Json;
// using Application.DTOs;
// using Application.Use_Classes.Commands.CompanyCommands;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using Newtonsoft.Json;
//
// namespace Partial.IntegrationTests.Companies;
//
// public class CompaniesControllerTests : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//
//     public CompaniesControllerTests(WebApplicationFactory<Program> factory)
//     {
//         _client = factory.CreateClient();
//     }
//
//     [Fact]
//     public async Task CreateCompany_ShouldReturnCreatedCompanyId()
//     {
//         // Arrange
//         var command = new CreateCompanyCommand
//         {
//             Name = "Test Company",
//             Email = "test@company.com",
//             PhoneNumber = "123456789",
//             City = "Test City",
//             Country = "Test Country",
//             Description = "Test description",
//             ProfilePicture = "profile.jpg"
//         };
//
//         // Act
//         var response = await _client.PostAsJsonAsync("/api/v1/Companies", command);
//         response.EnsureSuccessStatusCode();
//
//         // Assert
//         var responseString = await response.Content.ReadAsStringAsync();
//         var result = JsonConvert.DeserializeObject<Guid>(responseString);
//         Assert.NotEqual(Guid.Empty, result);
//     }
//
//     [Fact]
//     public async Task GetAllCompanies_ShouldReturnListOfCompanies()
//     {
//         // Act
//         var response = await _client.GetAsync("/api/v1/Companies");
//         response.EnsureSuccessStatusCode();
//
//         // Assert
//         var companies = await response.Content.ReadFromJsonAsync<List<CompanyDto>>();
//         Assert.NotNull(companies);
//         Assert.IsType<List<CompanyDto>>(companies);
//     }
//
//     [Fact]
//     public async Task GetCompanyById_ShouldReturnCompany_WhenCompanyExists()
//     {
//         // Arrange
//         var createCommand = new CreateCompanyCommand
//         {
//             Name = "Test Company",
//             Email = "test@company.com",
//             PhoneNumber = "123456789",
//             City = "Test City",
//             Country = "Test Country",
//             Description = "Test description",
//             ProfilePicture = "profile.jpg"
//         };
//         var createResponse = await _client.PostAsJsonAsync("/api/v1/Companies", createCommand);
//         var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());
//
//         // Act
//         var response = await _client.GetAsync($"/api/v1/Companies/{createdId}");
//         response.EnsureSuccessStatusCode();
//
//         // Assert
//         var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
//         Assert.NotNull(company);
//         Assert.Equal(createdId, company.Id);
//     }
//
//     [Fact]
//     public async Task UpdateCompany_ShouldReturnNoContent_WhenCompanyIsUpdated()
//     {
//         // Arrange
//         var createCommand = new CreateCompanyCommand
//         {
//             Name = "Test Company",
//             Email = "test@company.com",
//             PhoneNumber = "123456789",
//             City = "Test City",
//             Country = "Test Country",
//             Description = "Test description",
//             ProfilePicture = "profile.jpg"
//         };
//         var createResponse = await _client.PostAsJsonAsync("/api/v1/Companies", createCommand);
//         var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());
//
//         var updateCommand = new UpdateCompanyCommand
//         {
//             Id = createdId,
//             Name = "Updated Test Company",
//             Email = "updated@company.com",
//             PhoneNumber = "987654321",
//             City = "Updated City",
//             Country = "Updated Country",
//             Description = "Updated description",
//             ProfilePicture = "updated_profile.jpg"
//         };
//
//         // Act
//         var response = await _client.PutAsJsonAsync($"/api/v1/Companies/{createdId}", updateCommand);
//
//         // Assert
//         Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//     }
//
//     [Fact]
//     public async Task DeleteCompany_ShouldReturnNoContent_WhenCompanyIsDeleted()
//     {
//         // Arrange
//         var createCommand = new CreateCompanyCommand
//         {
//             Name = "Test Company",
//             Email = "test@company.com",
//             PhoneNumber = "123456789",
//             City = "Test City",
//             Country = "Test Country",
//             Description = "Test description",
//             ProfilePicture = "profile.jpg"
//         };
//         var createResponse = await _client.PostAsJsonAsync("/api/v1/Companies", createCommand);
//         var createdId = JsonConvert.DeserializeObject<Guid>(await createResponse.Content.ReadAsStringAsync());
//
//         // Act
//         var response = await _client.DeleteAsync($"/api/v1/Companies/{createdId}");
//
//         // Assert
//         Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//     }
// }