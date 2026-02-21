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
        var repository = new Mock<IDataRepository>(MockBehavior.Strict);
        repository.Setup(r=>r.ReadUserFromTheToken()).Returns(fakeUserContext);
        repository.Setup(r => r.GetUserForUpdateAsync(fakeUserContext.Subject, It.IsAny<CancellationToken>())).ReturnsAsync((UserDomain?)null);
        repository.Setup(r => r.AddUserForDbAsync(It.IsAny<UserDomain>()))
            .Returns(Task.CompletedTask);
        repository.Setup(r => r.DbSaveAsync())
            .Returns(Task.CompletedTask);
        var service = new CreateUserProfileService(repository.Object);

        //Act
        await service.CreateUser(ct);
        //Assert
        repository.Verify(r => r.AddUserForDbAsync(It.IsAny<UserDomain>()), Times.Once);
        repository.Verify(r => r.DbSaveAsync(), Times.Once);
        
    }
}
