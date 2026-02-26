using FluentAssertions;
using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Features.CreateUserProfile;
using UserService.Infrastructure;
using UserService.Tests.FakeBuilder;
using UserService.Tests.FakeRepository;

namespace UserService.Tests;

public class CreateUserProfileServiceTests
{
    private readonly FakeDataRepository _repository;
    private readonly CreateUserProfileService _service;
    public CreateUserProfileServiceTests()
    {
        _repository= new FakeDataRepository();
        _service = new CreateUserProfileService(_repository);
    }
    [Fact]
    public async Task Check_user_in_database_and_update_date_value_lastLogin()
    {
        var ct = TestContext.Current.CancellationToken;
        
        var existing=new FakeUserBuilder()
            .WithExternalId(Guid.Parse("81899d30-82e4-4db6-8b5d-669b05392232"))
            .Build();
        _repository.TokenSubject = existing.ExternalId;
        var oldDate = existing.LastLoginAt!.Value;
        _repository.Seed(existing);
        
        await _service.CreateUser(ct);
        
        existing.LastLoginAt.Should().NotBeNull();
        existing.LastLoginAt!.Value.Should().BeAfter(oldDate);
        _repository.AddUserCalls.Should().Be(0);
        _repository.SaveCalls.Should().Be(1);
    }
    [Fact]
    public async Task Create_domain_user_when_user_exists()
    {
        _repository.TokenSubject = Guid.NewGuid();
        _repository.TokenUserName = "alice";
        _repository.TokenEmail = "alice@test.com";
        var ct = TestContext.Current.CancellationToken;
        
        await _service.CreateUser(ct);

        var created = _repository.GetUser(_repository.TokenSubject);
        created.Should().NotBeNull();
        created.ExternalId.Should().Be(_repository.TokenSubject);
        created.Username.Should().Be(_repository.TokenUserName);
        created.Email.Should().Be(_repository.TokenEmail);
        created.LastLoginAt.Should().NotBeNull();
        _repository.AddUserCalls.Should().Be(1);
        _repository.SaveCalls.Should().Be(1);
    }
}
