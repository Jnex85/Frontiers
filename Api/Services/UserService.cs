using Api.Models;
using Api.Models.Exceptions;
using Api.Repository;

namespace Api.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<bool> InviteReviewerAsyncAsync(int userId);
        Task<User> RegisterUserAsync(RegisterUserInput registerUser);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<bool> InviteReviewerAsyncAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId) ?? throw new UserNotFoundException();

            var university = await _userRepository.GetUniversityByNameAsync(user.UniversityName);

            var isReviewer = university != null && university.Score >= 60 && user.NumberOfPublications > 3;
            if(isReviewer)
            {
                await _userRepository.SetUserAsReviewerAsync(userId);
            }
            return isReviewer;
        }

        public async Task<User> RegisterUserAsync(RegisterUserInput registerUser)
        {
            var userExists = await _userRepository.GetUserByUserNameAsync(registerUser.UserName);
            if(userExists != null)
            {
                throw new UserAlreadyRegisteredException();
            }

            return await _userRepository.AddUserAsync(registerUser.UserName, registerUser.UniversityName, registerUser.NumberOfPublications);
        }
    }
}