
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
using Microsoft.AspNetCore.Authorization;

namespace DailyWorkCoreApi.Controllers
{
    [Authorize]
    [Route("api/V1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _ipAddress;
        private readonly ILogger<UsersController> _logger;
        private readonly IOptions<SystemConfiguration> config;
        private readonly UsersService _UsersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MongoHelperService _MongoHelperService;
        string host = string.Empty;
        public UsersController(IOptions<SystemConfiguration> _config, ILogger<UsersController> logger, UsersService usersService,MongoHelperService mongoHelperService, IHttpContextAccessor httpContextAccessor)
        {
            this.config = _config;
            this._logger = logger;
            this._UsersService = usersService;
            this._MongoHelperService = mongoHelperService;
            _httpContextAccessor = httpContextAccessor;
            _ipAddress = CustomResponse.GetIPAddress(httpContextAccessor);
            _logger.LogInformation(_ipAddress + "User Controller loaded ");
            host = _httpContextAccessor.HttpContext.Request.Host.Value;
            _logger.LogInformation(_ipAddress + " Host Name " + host);
        }

        // GET: api/<RoadMapController>
        [HttpGet]
        public async Task<List<user>> Get()
        {
            List<user> lstUsers = new List<user>();
            try
            {
                await Task.Run(() =>
                {
                    
                    lstUsers = _UsersService.Get();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(_ipAddress + " Get All Users " + ex.ToString());
            }
            return lstUsers;
        }

        // [HttpGet("{userid:length(24)}", Name = "Getuser")]
        [HttpGet("Getuser")]
        public async Task<user> Getuser(string userid)
        {
            user User = new user();
            try
            {
                await Task.Run(() =>
                {
                    User = _UsersService.GetByUserId(userid);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(_ipAddress + "Get single user " + ex.ToString());
            }
            _logger.LogInformation(_ipAddress + "Get single user check " + userid + System.Text.Json.JsonSerializer.Serialize(User).ToString());
            return User;
        }
        [HttpPost]
        public ActionResult<user> Post(user userIn)
        {
            _logger.LogInformation(_ipAddress + "POST Called - Input Data: " + userIn.ToString());
            _UsersService.Create(userIn);

            return userIn;
        }

        [HttpPost("ValidateUser")]
        public HttpResponseMessage ValidateUser(string userid, string password)
        {
            HttpResponseMessage response;
            user _user = _UsersService.Validate(userid, password);
            if (_user == null)
            {
                return response = CustomResponse.GetResponseMessage<string>("Invalid User", HttpStatusCode.Unauthorized, ResponseType.JSON);
            }
            return response = CustomResponse.GetResponseMessage<user>(_user, HttpStatusCode.OK, ResponseType.JSON);
        }

        [HttpPut]
        public IActionResult Update(string userid, user userIn)
        {
            var user = _UsersService.GetByUserId(userid);

            if (user == null)
            {
                return NotFound();
            }

            _UsersService.Update(userid, userIn);

            return NoContent();
        }
        //  api/V1/users/id
        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(string id)
        {
            _logger.LogInformation(_ipAddress + " HttpDelete " + id);
            HttpResponseMessage response;
            var user = _UsersService.GetById(id);

            if (user == null)
            {
                return response = CustomResponse.GetResponseMessage<string>("Invalid User", HttpStatusCode.Unauthorized, ResponseType.JSON);
            }
            _logger.LogInformation(_ipAddress + " user found " + user.Userid);
            _UsersService.Remove(user._id);
           
            return response = CustomResponse.GetResponseMessage<user>(user, HttpStatusCode.OK, ResponseType.JSON);
        }

        

    }
     
}
