using API.DTOs;
using API.Services;
using Application.Users;
using Domain;
using Domain.Constant;
using Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly TokenService tokenService;
        private readonly EmailSender emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService, EmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.Users.Include(r => r.Restaurant).Include(r => r.Branch).FirstOrDefaultAsync(a => a.Email == loginDto.Email);
            if (user == null) return Unauthorized("Invalid email");
            if (!user.EmailConfirmed) return Unauthorized("Email not confirmed");
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                await SetRefreshToken(user);
                return await CreateUserObject(user);
            }

            return Unauthorized("Invalid password");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }

            if (await userManager.Users.AnyAsync(x => x.UserName == registerDto.Name))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            var user = new User
            {
                UserName = registerDto.Name,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Role = Domain.Enum.RoleEnum.Manager
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("Problem registering user");
            var origin = Request.Headers["origin"];
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify Email</a></p>";
            emailSender.SendEmail(user.Email, "Please verify email", message);
            return Ok("Registration success - please verify email");

            //if (result.Succeeded)
            //{
            //    await SetRefreshToken(user);
            //    return CreateUserObject(user);
            //}

            //return BadRequest("Problem registering user");
        }

        [AllowAnonymous]
        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded) return BadRequest("Could not verify email address");
            return Ok("Email confirmed - you can now login");
        }

        [AllowAnonymous]
        [HttpGet("resendEmailConfirmationLink")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();
            var origin = Request.Headers["origin"];
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify Email</a></p>";
            emailSender.SendEmail(user.Email, "Please verify email", message);
            return Ok("Email verification link resent");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await userManager.Users.Include(r => r.Restaurant).Include(r => r.Branch).FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            await SetRefreshToken(user);
            return await CreateUserObject(user);
        }

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await userManager.Users
                .Include(r => r.RefreshTokens).Include(r => r.Restaurant).Include(r => r.Branch)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();
            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            if (oldToken != null && !oldToken.IsActive) return Unauthorized();
            return await CreateUserObject(user);
        }

        private async Task SetRefreshToken(User user)
        {
            var refreshToken = tokenService.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        private async Task<UserDto> CreateUserObject(User user)
        {
            return new UserDto
            {
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Username = user.UserName,
                Roles = await userManager.GetRolesAsync(user),
                Token = tokenService.CreateToken(user)
            };
        }
    }
}
