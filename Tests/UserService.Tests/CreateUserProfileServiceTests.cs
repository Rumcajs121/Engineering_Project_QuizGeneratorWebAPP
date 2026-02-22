using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Features.CreateUserProfile;
using UserService.Infrastructure;

namespace UserService.Tests;

public class CreateUserProfileServiceTests
{
    [Fact]
    public async Task GiverNewUser_WhenCreatingNewUser_Creates()
    {
        //Arrange 
        var ct = CancellationToken.None;
        var fakeUserContext = new UserContextDto(
            Email:"rumcajs@example.com",
            Subject:Guid.NewGuid(),
            UserName:"rumcajs",
            IsAuthenticated:true
            );
        var dataTimeCheck = DateTime.UtcNow;
        var repository = new Mock<IDataRepository>(MockBehavior.Strict);
        repository.Setup(r=>r.ReadUserFromTheToken()).Returns(fakeUserContext);
        repository.Setup(r => r.GetUserForUpdateAsync(fakeUserContext.Subject, ct)).ReturnsAsync((UserDomain?)null);
        repository.Setup(r => r.AddUserForDbAsync(It.Is<UserDomain>(u =>
            u.ExternalId == fakeUserContext.Subject &&
            u.Username == fakeUserContext.UserName &&
            u.Email == fakeUserContext.Email &&
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
        repository.Verify(r => r.GetUserForUpdateAsync(fakeUserContext.Subject, ct), Times.Once);
        repository.Verify(r => r.AddUserForDbAsync(It.IsAny<UserDomain>()), Times.Once);
        repository.Verify(r => r.DbSaveAsync(), Times.Once);
        repository.VerifyNoOtherCalls();
        
    }
}
