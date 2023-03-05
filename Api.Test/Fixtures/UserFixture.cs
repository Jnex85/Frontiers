using Api.Models;

namespace Api.Test.Fixtures
{
    public static class UserFixture
    {
        public static List<User> GetTestUsers() =>
            new()
            {
                new User
                {
                    Id = 1,
                    NumberOfPublications = Helpers.Helpers.GenerateRandomNumber(10),
                    UserName = Helpers.Helpers.GenerateRandomString(),
                    UniversityName = Helpers.Helpers.GenerateRandomString()
                },
                new User
                {
                    Id = 2,
                    NumberOfPublications = Helpers.Helpers.GenerateRandomNumber(10),
                    UserName = Helpers.Helpers.GenerateRandomString(),
                    UniversityName = Helpers.Helpers.GenerateRandomString()
                },
                new User
                {
                    Id = 3,
                    NumberOfPublications =Helpers.Helpers.GenerateRandomNumber(10),
                    UserName = Helpers.Helpers.GenerateRandomString(),
                    UniversityName = Helpers.Helpers.GenerateRandomString()
                }
            };

        public static RegisterUserInput GetRegisterUserInput() => new()
        {
            NumberOfPublications = Helpers.Helpers.GenerateRandomNumber(10),
            UniversityName = Helpers.Helpers.GenerateRandomString(),
            UserName = Helpers.Helpers.GenerateRandomString()
        };

        public static User GetUser() => new()
        {
            NumberOfPublications = Helpers.Helpers.GenerateRandomNumber(10),
            Id = Helpers.Helpers.GenerateRandomNumber(1000),
            UserName = Helpers.Helpers.GenerateRandomString(),
            UniversityName = Helpers.Helpers.GenerateRandomString()
        };

        public static University GetUniversity() => new()
        {
            Id = Helpers.Helpers.GenerateRandomNumber(1000),
            Name = Helpers.Helpers.GenerateRandomString(),
            Score = Helpers.Helpers.GenerateRandomNumber(100, 50)
        };
    }
}