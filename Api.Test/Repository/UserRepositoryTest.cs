using Api.Models;
using Api.Repository;
using Api.Test.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Api.Test.Repository
{
    public class UserRepositoryTest
    {
        #region GetUsersAsync

        [Fact]
        public async Task GetUsersAsync_Should_ReturnListOfUsers()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var user = UserFixture.GetUser();
            user.Id = 1;
            await userRepository.AddUserAsync(user.UserName, user.UniversityName, user.NumberOfPublications);

            //act
            var sut = userRepository;
            var result = await sut.GetUsersAsync();

            //assert
            result.ToList().Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetUsersAsync_Should_ReturnListOfUsersEmpty()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());

            //act
            var sut = userRepository;
            var result = await sut.GetUsersAsync();

            //assert
            result.ToList().Count.Should().Be(0);
        }

        #endregion

        #region AddUserAsync

        [Fact]
        public async Task AddUserAsync_Should_ReturnUser()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var user = UserFixture.GetUser();
            user.Id = 1;

            //act
            var result = await userRepository.AddUserAsync(user.UserName, user.UniversityName, user.NumberOfPublications);

            //assert
            result.Id.Should().Be(user.Id);
            result.NumberOfPublications.Should().Be(user.NumberOfPublications);
            result.UserName.Should().Be(user.UserName);
            result.UniversityName.Should().Be(user.UniversityName);
        }

        #endregion

        #region GetUserByIdAsync

        [Fact]
        public async Task GetUserByIdAsync_Should_ReturnUser()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var user = UserFixture.GetUser();
            user.Id = 1;
            await userRepository.AddUserAsync(user.UserName, user.UniversityName, user.NumberOfPublications);

            //act
            var sut = userRepository;
            var result = await sut.GetUserByIdAsync(user.Id);

            //assert
            result.Id.Should().Be(user.Id);
            result.NumberOfPublications.Should().Be(user.NumberOfPublications);
            result.UserName.Should().Be(user.UserName);
            result.UniversityName.Should().Be(user.UniversityName);
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_ReturnUserEmpty()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());

            //act
            var sut = userRepository;
            var result = await sut.GetUserByIdAsync(1);

            //assert
            result.Should().BeNull();
        }

        #endregion

        #region GetUserByUserNameAsync

        [Fact]
        public async Task GetUserByUserNameAsync_Should_ReturnUser()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var user = UserFixture.GetUser();
            user.Id = 1;
            await userRepository.AddUserAsync(user.UserName, user.UniversityName, user.NumberOfPublications);

            //act
            var sut = userRepository;
            var result = await sut.GetUserByUserNameAsync(user.UserName);

            //assert
            result.Id.Should().Be(user.Id);
            result.NumberOfPublications.Should().Be(user.NumberOfPublications);
            result.UserName.Should().Be(user.UserName);
            result.UniversityName.Should().Be(user.UniversityName);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_Should_ReturnUserEmpty()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetUsersAsync())
                .ReturnsAsync(UserFixture.GetTestUsers());

            //act
            var sut = userRepository;
            var result = await sut.GetUserByUserNameAsync("pippo");

            //assert
            result.Should().BeNull();
        }

        #endregion

        #region GetUniversityByNameAsync

        [Fact]
        public async Task GetUniversityByNameAsync_Should_ReturnUniversity()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var university = UserFixture.GetUniversity();
            university.Id = 1;
            await userRepository.AddUniversityAsync(university.Name, university.Score);

            //act
            var sut = userRepository;
            var result = await sut.GetUniversityByNameAsync(university.Name);

            //assert
            result.Id.Should().Be(university.Id);
            result.Name.Should().Be(university.Name);
            result.Score.Should().Be(university.Score);
        }

        [Fact]
        public async Task GetUniversityByNameAsync_Should_ReturnUniversityEmpty()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());

            //act
            var sut = userRepository;
            var result = await sut.GetUniversityByNameAsync("pippo");

            //assert
            result.Should().BeNull();
        }

        #endregion

        #region AddUniversityAsync

        [Fact]
        public async Task AddUniversityAsync_Should_ReturnUniversity()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var university = UserFixture.GetUniversity();
            university.Id = 1;

            //act
            var result = await userRepository.AddUniversityAsync(university.Name, university.Score);

            //assert
            result.Id.Should().Be(university.Id);
            result.Name.Should().Be(university.Name);
            result.Score.Should().Be(university.Score);
        }

        #endregion

        #region SetUserAsReviewerAsync

        [Fact]
        public async Task SetUserAsReviewerAsync_Should_ReturnUser()
        {
            //arrange
            var userRepository = new UserRepository(GetContextWithInMemoryProvider());
            var user = UserFixture.GetUser();
            user.Id = 1;
            await userRepository.AddUserAsync(user.UserName, user.UniversityName, user.NumberOfPublications);

            //act
            var result = await userRepository.SetUserAsReviewerAsync(user.Id);

            //assert
            result.Id.Should().Be(user.Id);
            result.UserName.Should().Be(user.UserName);
            result.Reviewer.Should().BeTrue();
            result.NumberOfPublications.Should().Be(user.NumberOfPublications);
        }

        #endregion

        private static UserDBContext GetContextWithInMemoryProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<UserDBContext>();
            builder.UseInMemoryDatabase(Helpers.Helpers.GenerateRandomString())
                   .UseInternalServiceProvider(serviceProvider);

            return new UserDBContext(builder.Options);
        }
    }
}