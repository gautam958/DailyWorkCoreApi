
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyWorkCoreApi.Shared;
using DailyWorkCoreApi.Services;
using DailyWorkCoreApi.Model;
using System.Net.Http;
using System.Net;
using static DailyWorkCoreApi.Common.CustomResponse;
using DailyWorkCoreApi.Common;
using Microsoft.AspNetCore.Http;

namespace DailyWorkCoreApi.Controllers
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _ipAddress;
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<SystemConfiguration> config;
        private readonly UsersService _UsersService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserController(IOptions<SystemConfiguration> _config, ILogger<UserController> logger,UsersService usersService, IHttpContextAccessor httpContextAccessor)
        {
            this.config = _config;
            this._logger = logger;
            this._UsersService = usersService;
            this.httpContextAccessor = httpContextAccessor;
            _ipAddress= CustomResponse.GetIPAddress(httpContextAccessor);
            _logger.LogInformation(_ipAddress +"User Controller loaded ");
        }

        // GET: api/<RoadMapController>
        [HttpGet]
        public async Task<List<Users>> Get()
        { 
             List <Users> lstUsers = new List<Users>();
            try
            {   
                await Task.Run(() =>
                {
                    lstUsers = _UsersService.Get();
                }); 
            }
            catch (Exception ex)
            {
                _logger.LogError(_ipAddress+ " Get All Users " +ex.ToString());
            }
            return lstUsers;
        }

        // [HttpGet("{userid:length(24)}", Name = "Getuser")]
        [HttpGet("Getuser")]
        public async Task<Users> Getuser(string userid)
        {
            Users User = new  Users();
            try
            {
                await Task.Run(() =>
                {
                    User = _UsersService.Get(userid);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(_ipAddress + "Get single user " +ex.ToString());
            }
            return User;
        }
        [HttpPost]
        public ActionResult<Users> Create(Users userIn)
        {
            _UsersService.Create(userIn);

            return userIn;
        }

        [HttpPost("ValidateUser")]
        public HttpResponseMessage ValidateUser(string userid, string password)
        {
            HttpResponseMessage response;
            Users user= _UsersService.Validate(userid, password);
            if(user==null)
            {
                return response = CustomResponse.GetResponseMessage<string>("Invalid User", HttpStatusCode.Unauthorized, ResponseType.JSON);
            }
            return response = CustomResponse.GetResponseMessage<Users>(user, HttpStatusCode.OK, ResponseType.JSON);
        }

        [HttpPut]
        public IActionResult Update(string userid, Users userIn)
        {
            var user = _UsersService.Get(userid);

            if (user == null)
            {
                return NotFound();
            }

            _UsersService.Update(userid, userIn);

            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(string userid)
        {
            var user = _UsersService.Get(userid);

            if (user == null)
            {
                return NotFound();
            }

            _UsersService.Remove(user.Userid);

            return NoContent();
        }

        

    }
     
}
