using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Features.CreateUserProfile;
using UserService.Infrastructure;
using UserService.Tests.FakeRepository;

namespace UserService.Tests;

public class CreateUserProfileServiceTests
{
    private readonly UserContextDto _fakeUserContext =
        new UserContextDto(
            Email: "rumcajs@example.com",
            Subject: Guid.NewGuid(),
            UserName: "rumcajs",
            IsAuthenticated: true
        );
    private readonly FakeDataRepository _repository;
    private readonly CreateUserProfileService _service;
    public CreateUserProfileServiceTests()
    {
        _repository = new FakeDataRepository();
        _service = new CreateUserProfileService(_repository);
    }
    
    // TODO: Replace Mock with Fake + Spy
    
    [Fact]
    public async Task GiverNewUser_WhenCreatingNewUser_Creates()
    {
        
        //Arrange 
        var ct = CancellationToken.None;
        var dataTimeCheck = DateTime.UtcNow;
        var repository = new Mock<IDataRepository>(MockBehavior.Strict);
        repository.Setup(r=>r.ReadUserFromTheToken()).Returns(_fakeUserContext);
        repository.Setup(r => r.GetUserForUpdateAsync(_fakeUserContext.Subject, ct)).ReturnsAsync((UserDomain?)null);
        repository.Setup(r => r.AddUserForDbAsync(It.Is<UserDomain>(u =>
            u.ExternalId == _fakeUserContext.Subject &&
            u.Username == _fakeUserContext.UserName &&
            u.Email == _fakeUserContext.Email &&
            u.PrivilegeUserDomain == PrivilegesUserDomain.User &&
            u.IsActive == true &&
            u.CreatedAt >= dataTimeCheck &&
            u.LastLoginAt.HasValue &&
            u.LastLoginAt.Value >= dataTimeCheck
        ))).Returns(Task.CompletedTask);
        repository.Setup(r => r.DbSaveAsync())
            .Returns(Task.CompletedTask);
        var service = new CreateUserProfileService(repository.Object);

        //Act
        await service.CreateUser(ct);
        //Assert
        repository.Verify(r => r.ReadUserFromTheToken(), Times.Once);
        repository.Verify(r => r.GetUserForUpdateAsync(_fakeUserContext.Subject, ct), Times.Once);
        repository.Verify(r => r.AddUserForDbAsync(It.IsAny<UserDomain>()), Times.Once);
        repository.Verify(r => r.DbSaveAsync(), Times.Once);
        repository.VerifyNoOtherCalls();
        
    }

    [Fact]
    public async Task CheckUser_WhenUserLogIn_ChangeLastLogin()
    {
        //Arrange 
        var ct = CancellationToken.None;
        var existing = new UserDomain { ExternalId = _fakeUserContext.Subject, Username = _fakeUserContext.UserName, Email = _fakeUserContext.Email, PrivilegeUserDomain = PrivilegesUserDomain.User, IsActive = true, LastLoginAt = DateTime.UtcNow.AddDays(-7) };
        
        var repository = new Mock<IDataRepository>(MockBehavior.Strict);
        repository.Setup(r => r.ReadUserFromTheToken()).Returns(_fakeUserContext);
        repository.Setup(r => r.GetUserForUpdateAsync(_fakeUserContext.Subject, ct)).ReturnsAsync(existing);
        repository.Setup(r => r.DbSaveAsync()).Returns(Task.CompletedTask);
        
        var service = new CreateUserProfileService(repository.Object);
        var before = existing.LastLoginAt;
        await service.CreateUser(ct);
        
        Assert.True(existing.LastLoginAt.HasValue);
        Assert.True(existing.LastLoginAt.Value > before!.Value);
        repository.Verify(r => r.ReadUserFromTheToken(), Times.Once);
        repository.Verify(r => r.GetUserForUpdateAsync(_fakeUserContext.Subject, ct), Times.Once);
        repository.Verify(r => r.DbSaveAsync(), Times.Once);
        repository.VerifyNoOtherCalls();
    }
    
}
