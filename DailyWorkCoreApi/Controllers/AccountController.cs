using AuthenticationPlugin;
using DailyWorkCoreApi.Model;
using DailyWorkCoreApi.Services;
using DailyWorkCoreApi.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyWorkCoreApi.Controllers
{
    [Route("api/V1/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        private readonly MongoHelperService _mongoHelperService;
        private readonly UsersService _UsersService;
        public AccountController(IConfiguration configuration, MongoHelperService mongoHelperService, UsersService UsersService)
        {
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _mongoHelperService = mongoHelperService;
            _UsersService = UsersService;
        }

        // POST api/<AccountController>
        [HttpPost]
        public IActionResult Login([FromBody] user _user)
        {
            var validUser = _UsersService.GetByUserId(_user.Userid);
            if (validUser==null)
            {
                return NotFound();
            }

           // string hashedPassword = SecurePasswordHasherHelper.Hash(validUser.Password);

            if (!SecurePasswordHasherHelper.Verify(_user.Password, validUser.Password))
            {
                return Unauthorized();
            }
            var claims = new[]
                 {
               new Claim(JwtRegisteredClaimNames.Email, _user.Userid),
               new Claim(ClaimTypes.Email, _user.Userid),
             };
                var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                Userid = _user._id
            }) ;
            
        }
        [HttpPost]
        public IActionResult Create([FromBody] user _user)
        {
            _user.Password = SecurePasswordHasherHelper.Hash(_user.Password);
            _UsersService.Create(_user);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
