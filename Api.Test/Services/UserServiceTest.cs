using Api.Models;
using Api.Models.Exceptions;
using Api.Repository;
using Api.Services;
using Api.Test.Fixtures;
using FluentAssertions;
using Moq;

namespace Api.Test.Controllers
{
    public class UserServiceTest
    {
        #region GetAllUsersAsync

        [Fact]
        public async Task GetAllUsersAsync_OnSuccess_ReturnsListOfUser()
        {

            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetUsersAsync())
                .ReturnsAsync(UserFixture.GetTestUsers());

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.GetAllUsersAsync();

            //assert
            result.ToList().Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllUsersAsync_OnSuccess_ReturnsListOfUserEmpty()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetUsersAsync())
                .ReturnsAsync(new List<Models.User>());

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.GetAllUsersAsync();

            //assert
            result.ToList().Should().HaveCount(0);
        }

        #endregion

        #region InviteReviewerAsyncAsync

        [Fact]
        public async Task InviteReviewerAsyncAsync_WhenScoreAndPublicationsMeetRequirements_ReturnsTrue()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();
            returnedUser.NumberOfPublications = 4;
            returnedUser.Reviewer = true;
            var returnedUniversity = UserFixture.GetUniversity();
            returnedUniversity.Name = returnedUser.UniversityName;
            returnedUniversity.Score = 60;

            mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(returnedUser.Id))
                .ReturnsAsync(returnedUser);
            mockUserRepository
                .Setup(repo => repo.GetUniversityByNameAsync(returnedUniversity.Name))
                .ReturnsAsync(returnedUniversity);

            mockUserRepository
                .Setup(repo => repo.SetUserAsReviewerAsync(returnedUser.Id))
                .ReturnsAsync(returnedUser);

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.InviteReviewerAsyncAsync(returnedUser.Id);

            //assert
            result.Should().BeTrue();
            mockUserRepository.Verify(v => v.SetUserAsReviewerAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task InviteReviewerAsyncAsync_WhenScoreNotMeetRequirements_ReturnsFalse()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();
            returnedUser.NumberOfPublications = 4;
            var returnedUniversity = UserFixture.GetUniversity();
            returnedUniversity.Name = returnedUser.UniversityName;
            returnedUniversity.Score = 40;

            mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(returnedUser.Id))
                .ReturnsAsync(returnedUser);
            mockUserRepository
                .Setup(repo => repo.GetUniversityByNameAsync(returnedUniversity.Name))
                .ReturnsAsync(returnedUniversity);

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.InviteReviewerAsyncAsync(returnedUser.Id);

            //assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task InviteReviewerAsyncAsync_WhenPublicationsNotMeetRequirements_ReturnsFalse()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();
            returnedUser.NumberOfPublications = 1;
            var returnedUniversity = UserFixture.GetUniversity();
            returnedUniversity.Name = returnedUser.UniversityName;
            returnedUniversity.Score = 80;

            mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(returnedUser.Id))
                .ReturnsAsync(returnedUser);
            mockUserRepository
                .Setup(repo => repo.GetUniversityByNameAsync(returnedUniversity.Name))
                .ReturnsAsync(returnedUniversity);

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.InviteReviewerAsyncAsync(returnedUser.Id);

            //assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task InviteReviewerAsyncAsync_WhenScoreAndPublicationsNotMeetRequirements_ReturnsFalse()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();
            returnedUser.NumberOfPublications = 2;
            var returnedUniversity = UserFixture.GetUniversity();
            returnedUniversity.Name = returnedUser.UniversityName;
            returnedUniversity.Score = 40;

            mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(returnedUser.Id))
                .ReturnsAsync(returnedUser);
            mockUserRepository
                .Setup(repo => repo.GetUniversityByNameAsync(returnedUniversity.Name))
                .ReturnsAsync(returnedUniversity);

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.InviteReviewerAsyncAsync(returnedUser.Id);

            //assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task InviteReviewerAsyncAsync_WhenUserNotFound_ThrowsUserNotFoundException()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();

            mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(returnedUser.Id))
                .ReturnsAsync(null as User);

            //act
            var sut = new UserService(mockUserRepository.Object);

            //assert
            await sut.Invoking(x=>x.InviteReviewerAsyncAsync(returnedUser.Id)).Should().ThrowAsync<UserNotFoundException>();
        }

        #endregion

        #region RegisterUserAsync

        [Fact]
        public async Task RegisterUserAsync_WhenUserFound_ThrowsUserAlreadyRegisteredException()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();

            mockUserRepository
                .Setup(repo => repo.GetUserByUserNameAsync(returnedUser.UserName))
                .ReturnsAsync(returnedUser);

            //act
            var sut = new UserService(mockUserRepository.Object);

            //assert
            await sut.Invoking(x => x.RegisterUserAsync(new RegisterUserInput
            {
                UserName = returnedUser.UserName,
                NumberOfPublications = returnedUser.NumberOfPublications,
                UniversityName = returnedUser.UniversityName,
            })).Should().ThrowAsync<UserAlreadyRegisteredException>();
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserNotFound_ReturnsNewUserRegistered()
        {
            //arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var returnedUser = UserFixture.GetUser();

            mockUserRepository
                .Setup(repo => repo.GetUserByUserNameAsync(returnedUser.UserName))
                .ReturnsAsync(null as User);

            mockUserRepository
                .Setup(repo => repo.AddUserAsync(returnedUser.UserName, returnedUser.UniversityName, returnedUser.NumberOfPublications))
                .ReturnsAsync(returnedUser);

            //act
            var sut = new UserService(mockUserRepository.Object);
            var result = await sut.RegisterUserAsync(new RegisterUserInput
            {
                UserName = returnedUser.UserName,
                NumberOfPublications = returnedUser.NumberOfPublications,
                UniversityName = returnedUser.UniversityName,
            });

            //assert
            result.Should().Be(returnedUser);
        }
        #endregion
    }
}