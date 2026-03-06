using FluentAssertions;
using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Features.CreateUserProfile;
using UserService.Features.EditProfile;
using UserService.Infrastructure;
using UserService.Tests.FakeBuilder;
using UserService.Tests.FakeRepository;

namespace UserService.Tests;

public class UserServiceUnitsTests
{
    [Fact]
    public async Task Check_user_in_database_and_update_date_value_lastLogin()
    {
        var ct = TestContext.Current.CancellationToken;
        var repository= new FakeDataRepository();
        var service = new UserServiceUnits(repository);
        var existing=new FakeUserBuilder()
            .WithExternalId(Guid.Parse("81899d30-82e4-4db6-8b5d-669b05392232"))
            .Build();
        repository.TokenSubject = existing.ExternalId;
        var oldDate = existing.LastLoginAt!.Value;
        repository.Seed(existing);
        
        await service.CreateUser(ct);
        
        existing.LastLoginAt.Should().NotBeNull();
        existing.LastLoginAt!.Value.Should().BeAfter(oldDate);
        repository.AddUserCalls.Should().Be(0);
        repository.SaveCalls.Should().Be(1);
    }
    [Fact]
    public async Task Create_domain_user_when_user_exists()
    {
        var repository= new FakeDataRepository();
        var service = new UserServiceUnits(repository);
        repository.TokenSubject = Guid.NewGuid();
        repository.TokenUserName = "alice";
        repository.TokenEmail = "alice@test.com";
        var ct = TestContext.Current.CancellationToken;
        
        await service.CreateUser(ct);

        var created = repository.GetUser(repository.TokenSubject);
        created.Should().NotBeNull();
        created.ExternalId.Should().Be(repository.TokenSubject);
        created.Username.Should().Be(repository.TokenUserName);
        created.Email.Should().Be(repository.TokenEmail);
        created.LastLoginAt.Should().NotBeNull();
        repository.AddUserCalls.Should().Be(1);
        repository.SaveCalls.Should().Be(1);
    }

    [Fact]
    public async Task EditProfile_for_new_data()
    {
        var repository= new FakeDataRepository();
        var service = new EditProfileService(repository);
        var ct = TestContext.Current.CancellationToken;
        var existing = new FakeUserBuilder().WithExternalId(Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be")).Build();
        repository.TokenSubject = existing.ExternalId;
        repository.Seed(existing);
        var newUserName="newUserName";
        var newEmail="newEmail";
        var newPrivilegeUserDomain = PrivilegesUserDomain.Operator;
        var newIsActive=true;
        var dto = new EditProfileDto
        {
            PrivilegeUserDomain = newPrivilegeUserDomain,
            IsActive = newIsActive,
            Username = newUserName,
            Email = newEmail
        };
        await service.EditProfile(dto,ct);
        
        var newUser = repository.GetUser(repository.TokenSubject);
        newUser.Should().NotBeNull();
        newUser.Username.Should().Be(newUserName);
        newUser.Email.Should().Be(newEmail);
        newUser.PrivilegeUserDomain.Should().Be(newPrivilegeUserDomain);
        newUser.IsActive.Should().Be(newIsActive);
        repository.SaveCalls.Should().Be(1);
        repository.ReadSubFromTokenCall.Should().Be(1);
    }

    [Fact]
    public async Task Check_if_condition_in_EditedProfile()
    {
        var repository= new FakeDataRepository();
        var service = new EditProfileService(repository);
        var ct = TestContext.Current.CancellationToken;
        var existing=new FakeUserBuilder()
            .WithExternalId(Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be"))
            .Build();
        repository.TokenSubject = existing.ExternalId;
        repository.Seed(existing);
        string? newUserName=null;
        string? newEmail=null;
        bool? newIsActive=null;
        PrivilegesUserDomain? newPrivilegeUserDomain=null;
        var dto = new EditProfileDto
        {
            IsActive = newIsActive,
            Username = newUserName,
            Email = newEmail,
            PrivilegeUserDomain=newPrivilegeUserDomain
        };
        await service.EditProfile(dto,ct);
        var checkUser = repository.GetUser(repository.TokenSubject);
        checkUser.Should().NotBeNull();
        checkUser.Username.Should().Be(existing.Username);
        checkUser.Email.Should().Be(existing.Email);
        checkUser.PrivilegeUserDomain.Should().Be(existing.PrivilegeUserDomain);
        checkUser.IsActive.Should().Be(existing.IsActive);
        repository.ReadSubFromTokenCall.Should().Be(1);
        repository.SaveCalls.Should().Be(1);
    }
}
