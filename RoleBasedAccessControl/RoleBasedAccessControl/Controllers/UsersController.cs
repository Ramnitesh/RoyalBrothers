using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccessControl.Interface;
using RoleBasedAccessControl.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MUsers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoleBasedAccessControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private Library.APIResponse _response = new Library.APIResponse();
        private IUsers _iUser;
        private IHostingEnvironment _hostingEnvironment;
        public UsersController(IUsers iUser, IHostingEnvironment hostingEnvironment)
        {
            this._iUser = iUser;
            _hostingEnvironment = hostingEnvironment;

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
            {
                _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }

        private string GetClaim(string type)
        {
            ClaimsIdentity LoggedUserGuididentity = (ClaimsIdentity)User.Identity;
            return LoggedUserGuididentity.Claims.Where(c => c.Type == type).Select(c => c.Value).SingleOrDefault();
        }
        private Library.APIResponse SessionExpired()
        {
            _response.ResponseContent = "Session Expired";
            _response.StatusCode = 401;
            return _response;
        }

        [Route("AddNewUser")]
        [HttpPost]
        [Authorize(Roles = "Royal Brothers Admin, System Admin")]
        public async Task<IActionResult> AddNewUser([FromBody] MUsersDetails userdetails)
        {
            int LoggedUserID = Convert.ToInt32(GetClaim("UserId"));
            if (LoggedUserID == 0)
            {
                _response = SessionExpired();
            }
            else
            {
                _response = await _iUser.AddNewUser(Request.Headers["User_Password"], userdetails);
            }
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
        [Route("GetUserDetails")]
        [HttpGet]
        [Authorize(Roles = "Royal Brothers Admin, System Admin, Customer Admin, Customer Member, User")]
        public async Task<IActionResult> GetUserDetails(int userid = 0)
        {
            int LoggedUserID = Convert.ToInt32(GetClaim("UserId"));
            if (LoggedUserID == 0)
            {
                _response = SessionExpired();
            }
            else
            {
                _response = await _iUser.GetUserDetails(userid);
            }
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
        [Route("UpdateUserDetails")]
        [HttpPost]
        [Authorize(Roles = "Royal Brothers Admin, System Admin, Customer Admin, Customer Member")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] MUsersDetails usersDetails)
        {
            int LoggedUserID = Convert.ToInt32(GetClaim("UserId"));
            if (LoggedUserID == 0)
            {
                _response = SessionExpired();
            }
            else
            {
                _response = await _iUser.UpdateUserDetails(usersDetails);
            }
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
        [Route("DeleteUserDetails")]
        [HttpDelete]
        [Authorize(Roles = "Royal Brothers Admin, System Admin, Customer Admin")]
        public async Task<IActionResult> DeleteUserDetails(int userid)
        {
            int LoggedUserID = Convert.ToInt32(GetClaim("UserId"));
            if (LoggedUserID == 0)
            {
                _response = SessionExpired();
            }
            else
            {
                _response = await _iUser.DeleteUserDetails(userid);
            }
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
    }
}
