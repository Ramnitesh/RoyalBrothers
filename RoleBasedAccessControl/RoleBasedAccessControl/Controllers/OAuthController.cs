using RoleBasedAccessControl.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoleBasedAccessControl.Interface;
using System.Security.Claims;

namespace RoleBasedAccessControl.Controllers
{
    public class OAuthController : Controller
    {
        private Library.APIResponse _response;
        private IOAuth _iOAuth;
        public OAuthController(IOAuth iOAuth)
        {
            this._iOAuth = iOAuth;
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

        //[Route("Login")]
        //[HttpPost]
        public IActionResult Login()
        {
            _response = _iOAuth.Login(Request.Headers["UserCredentials_UserName"], Request.Headers["UserCredentials_Password"], 1);
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
        //[Route("Logout")]
        //[HttpPost]
        public IActionResult Logout()
        {
            _response = _iOAuth.Logout(Request);
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult temp()
        {
            _response = _iOAuth.Logout(Request);
            return StatusCode(Convert.ToInt16(_response.StatusCode), _response.ResponseContent);
        }
    }
}
