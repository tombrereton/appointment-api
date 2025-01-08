using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Web.Api.Domain.Accounts;
using Teeitup.Web.Api.Features.Accounts;
using Teeitup.Web.Api.Infrastructure.Database;
using Teeitup.Web.Api.IntegrationTests.Helpers;

namespace Teeitup.Web.Api.IntegrationTests.Features;

public class GetAccountShould(AppointerWebApplicationFactory<Program> factory)
    : IClassFixture<AppointerWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task GetExistingAccount()
    {
        // arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        var cancellationToken = cts.Token;

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var userAccount = UserAccount.Create("John Doe");
        await dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var client = _factory.CreateClient();
        var url = "api/accounts/" + userAccount.Id;

        // act
        var result = await client.GetAsync(url, cancellationToken);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<GetAccount.GetAccountResponse>(cancellationToken);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.FullName.Should().Be(userAccount.FullName);
    }
}