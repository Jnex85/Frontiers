using Api.Controllers;
using Api.Models;
using Api.Models.Exceptions;
using Api.Repository;
using Api.Services;
using Api.Test.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Test.Controllers
{
    public class UserControllerTest
    {
        #region GetUsersAsync

        [Fact]
        public async Task GetUsersAsync_OnSuccess_ReturnsStatusCode200()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(UserFixture.GetTestUsers());

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (OkObjectResult)await sut.GetUsersAsync();

            //assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetUsersAsync_OnSuccess_InvokesUserServiceExactlyOnce()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(new List<User>());

            //act
            var sut = new UserController(mockUserService.Object);
            var result = await sut.GetUsersAsync();

            //assert
            mockUserService.Verify(
                service => service.GetAllUsersAsync(),
                Times.Once());
        }

        [Fact]
        public async Task GetUsersAsync_OnSuccess_ReturnsListOfUsers()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var returnedUsers = UserFixture.GetTestUsers();

            mockUserService.
                Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(returnedUsers);

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (OkObjectResult)await sut.GetUsersAsync();

            //assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().Be(returnedUsers);
        }

        [Fact]
        public async Task GetUsersAsync_OnNoUsersFound_Return404()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.
                Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(new List<User>());

            //act
            var sut = new UserController(mockUserService.Object);
            var result = await sut.GetUsersAsync();

            //assert
            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(404);
        }

        #endregion

        #region PutRegisterUserAsync

        [Fact]
        public async Task PutRegisterUserAsync_OnSuccess_ReturnsStatusCode200()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userInput = UserFixture.GetRegisterUserInput();
            var returnedUser = UserFixture.GetUser();
            mockUserService
                .Setup(service => service.RegisterUserAsync(userInput))
                .ReturnsAsync(returnedUser);

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (OkObjectResult)await sut.PutRegisterUserAsync(userInput);

            //assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            result.Value.Should().Be(returnedUser);
        }

        [Fact]
        public async Task PutRegisterUserAsync_OnFailure_ReturnsStatusCode400()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userInput = UserFixture.GetRegisterUserInput();
            mockUserService
                .Setup(service => service.RegisterUserAsync(userInput))
                .ReturnsAsync(null as User);

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (BadRequestResult)await sut.PutRegisterUserAsync(userInput);

            //assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PutRegisterUserAsync_OnUserAlreadyRegistered_ReturnsStatusCode422()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userInput = UserFixture.GetRegisterUserInput();
            mockUserService
                .Setup(service => service.RegisterUserAsync(userInput)).Throws<UserAlreadyRegisteredException>();

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (UnprocessableEntityObjectResult)await sut.PutRegisterUserAsync(userInput);

            //assert
            result.StatusCode.Should().Be(422);
            result.Value.Should().Be("user already registered!");
        }

        [Fact]
        public async Task PutRegisterUserAsync_OnGenericError_ReturnsStatusCode400()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userInput = UserFixture.GetRegisterUserInput();
            mockUserService
                .Setup(service => service.RegisterUserAsync(userInput)).Throws<Exception>();

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (ObjectResult)await sut.PutRegisterUserAsync(userInput);

            //assert
            result.StatusCode.Should().Be(400);
        }

        #endregion

        #region PutInviteReviewerAsync

        [Fact]
        public async Task PutInviteReviewerAsync_OnSuccess_ReturnsStatusCode200()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var mockUserRepository = new Mock<IUserRepository>();
            var userId = 1;
            var userFound = UserFixture.GetUser();
            var university = UserFixture.GetUniversity();
            mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userFound);
            mockUserRepository.Setup(repo => repo.GetUniversityByNameAsync(userFound.UniversityName)).ReturnsAsync(university);

            mockUserService
                .Setup(service => service.InviteReviewerAsyncAsync(userId))
                .ReturnsAsync(true);

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (OkObjectResult)await sut.PutInviteReviewerAsync(userId);

            //assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            result.Value.Should().Be("The Invitation was successful!");
        }

        [Fact]
        public async Task PutInviteReviewerAsync_OnSuccess_ReturnsStatusCode200_InvitationDeclined()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var mockUserRepository = new Mock<IUserRepository>();
            var userId = 1;
            var userFound = UserFixture.GetUser();
            var university = UserFixture.GetUniversity();
            mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userFound);
            mockUserRepository.Setup(repo => repo.GetUniversityByNameAsync(userFound.UniversityName)).ReturnsAsync(university);

            mockUserService
                .Setup(service => service.InviteReviewerAsyncAsync(userId))
                .ReturnsAsync(false);

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (OkObjectResult)await sut.PutInviteReviewerAsync(userId);

            //assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            result.Value.Should().Be("Invitation refused,the minimum requirements are not satisfied!");
        }

        [Fact]
        public async Task PutInviteReviewerAsync_OnUserNotFound_ReturnsStatusCode422()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userId = 1;
            mockUserService
                .Setup(service => service.InviteReviewerAsyncAsync(userId)).Throws<UserNotFoundException>();

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (UnprocessableEntityObjectResult)await sut.PutInviteReviewerAsync(userId);

            //assert
            result.StatusCode.Should().Be(422);
            result.Value.Should().Be("user not found!");
        }

        [Fact]
        public async Task PutInviteReviewerAsync_OnGenericError_ReturnsStatusCode400()
        {
            //arrange
            var mockUserService = new Mock<IUserService>();
            var userInput = UserFixture.GetRegisterUserInput();
            mockUserService
                .Setup(service => service.RegisterUserAsync(userInput)).Throws<Exception>();

            //act
            var sut = new UserController(mockUserService.Object);
            var result = (ObjectResult)await sut.PutRegisterUserAsync(userInput);

            //assert
            result.StatusCode.Should().Be(400);
        }

        #endregion
    }
}