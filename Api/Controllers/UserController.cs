using Api.Models;
using Api.Models.Exceptions;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _usersService;

        public UserController(IUserService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _usersService.GetAllUsersAsync();

            if (users.Any())
            {
                return Ok(users);
            }

            return NotFound();
        }

        [HttpPut(Name = "RegisterUser")]
        [Route("api/User/RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PutRegisterUserAsync(RegisterUserInput registerUser)
        {
            try
            {
                var registeredUser = await _usersService.RegisterUserAsync(registerUser);
                if (registeredUser != null)
                {
                    return Ok(registeredUser);
                }

                return BadRequest();
            }
            catch(UserAlreadyRegisteredException)
            {
                return UnprocessableEntity("user already registered!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "InviteReviewer")]
        [Route("api/User/InviteReviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PutInviteReviewerAsync(int userId)
        {
            try
            {
                var invited = await _usersService.InviteReviewerAsyncAsync(userId);
                var result = invited ? "The Invitation was successful!" : "Invitation refused,the minimum requirements are not satisfied!";
                return Ok(result);
            }
            catch(UserNotFoundException)
            {
                return UnprocessableEntity("user not found!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}