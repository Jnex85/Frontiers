using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> AddUserAsync(string userName, string universityName, int numberOfPublications);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<University> GetUniversityByNameAsync(string universityName);
        Task<University> AddUniversityAsync(string universityName, int score);
    }

    public class UserRepository : IUserRepository
    {
        private readonly UserDBContext _dbContext;

        public UserRepository(UserDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> AddUserAsync(string userName, string universityName, int numberOfPublications)
        {
            var user = new User
            {
                Id = await GetNextUserId(),
                NumberOfPublications = numberOfPublications,
                UserName = userName,
                UniversityName = universityName
            };

            var universityExists = await _dbContext.Universities.Where(x=>x.Name.ToLowerInvariant().Equals(universityName)).FirstOrDefaultAsync();
            if(universityExists == null)
            {
                await AddUniversityAsync(universityName, Helpers.Helpers.GenerateRandomNumber(100, 50));
            }

            var result = await _dbContext.Users.AddAsync(user);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task<University> AddUniversityAsync(string universityName, int score)
        {
            var result = await _dbContext.Universities.AddAsync(new University
            {
                Id = await GetNextUniversityId(),
                Name = universityName,
                Score = score
            });

            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.Where(x=>x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<University> GetUniversityByNameAsync(string universityName)
        {
            return await _dbContext.Universities.Where(x => x.Name.ToLowerInvariant().Equals(universityName.ToLowerInvariant())).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _dbContext.Users.Where(x => x.UserName.ToLowerInvariant().Equals(userName.ToLowerInvariant())).FirstOrDefaultAsync();
        }

        private async Task<int> GetNextUserId()
        {
            return await _dbContext.Users.CountAsync() + 1;
        }

        private async Task<int> GetNextUniversityId()
        {
            return await _dbContext.Universities.CountAsync() + 1;
        }
    }
}