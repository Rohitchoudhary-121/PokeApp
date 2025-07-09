using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PokemonWebApp.Application.Models.Account;
using PokemonWebApp.Application.Models.Common;
using PokemonWebApp.Domain.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace PokemonWebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
        }


        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <returns>Registration Result</returns>
        /// <param name="model">Register Request Model</param>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserModel model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("Email and password is required.");

                if (string.IsNullOrEmpty(model.Email))
                    throw new ArgumentNullException("Email is required.");

                if (string.IsNullOrEmpty(model.Password))
                    throw new ArgumentNullException("Password is required.");

                var newUser = new ApplicationUser { FirstName = model.FirstName, UserName = model.Email, Email = model.Email, CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow };
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                return new BaseResult
                {
                    ResponseStatusCode = HttpStatusCode.Created,
                    Message = $"{model.Email} registered successfully",
                };
            }
            catch (Exception ex)
            {
                return new BaseResult
                {
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message },
                    ResponseStatusCode = HttpStatusCode.BadRequest
                };
            }
        }


        /// <summary>
        /// Login existing user.
        /// </summary>
        /// <returns>Login Result</returns>
        /// <param name="model">Login Request Model</param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<GenericBaseResult<AuthResponseModel>>> Login([FromBody] LoginModel model)
        {
            try
            {
                // Check command values
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                    throw new ArgumentNullException("All fields are required: Email, Password.");

                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                    throw new Exception("Invalid email.");

                // Password verification
                var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, true, lockoutOnFailure: true);

                if (result.IsLockedOut)
                    throw new Exception("Login locked after over 5 failed password attempts");

                if (!result.Succeeded)
                    throw new Exception("Invalid email or password.");

                // Token generation
                var jwtTokenResult = await GetToken(user);

                return new GenericBaseResult<AuthResponseModel>(jwtTokenResult);

            }
            catch (Exception ex)
            {
                return new GenericBaseResult<AuthResponseModel>(null)
                {
                    ResponseStatusCode = HttpStatusCode.Unauthorized,
                    Errors = new List<string> { ex.Message },
                    Message = ex.Message
                };
            }
        }

        private async Task<AuthResponseModel> GetToken(ApplicationUser user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                     };

            var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        claims: claims,
                        notBefore: _jwtOptions.NotBefore,
                        expires: _jwtOptions.Expiration,
            signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResponseModel
            {
                AccessToken = encodedJwt,
                Expires = (int)_jwtOptions.ValidFor.TotalSeconds
            };

        }
    }
}
