using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RoleBasedAccessControl.Interface;
using RoleBasedAccessControl.Model;
using RoleBasedAccessControl.Models;
using RoleBasedAccessControl.POCO;
using RoleBasedAccessControl.Queries.OAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MOAuth;

namespace RoleBasedAccessControl.Repository
{
    public class ROAuth : IOAuth
    {
        private readonly Library.APIResponse responseMsg;
        private Library.APIResponse _response = new Library.APIResponse();
        public readonly string AOPDBConnectionString;
        private readonly IOAuthDataAccess _dataAccessAOP;
        public ROAuth(IOAuthDataAccess AOPDataAccess)
        {
            _dataAccessAOP = AOPDataAccess;
            responseMsg = new Library.APIResponse();
        }

        //SqlConnectionClass connection = new SqlConnectionClass(Poco.AppSettings.BIBConnectionString);

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public Library.APIResponse Login(string userName, string password, int roleId)
        {
            try
            {
                string JsonString = string.Empty;


                LoginResponse userdetails = _dataAccessAOP.ValidateUser(userName, password, roleId);

                if (userdetails != null && userdetails.UserId != 0)
                {
                    List<Roles> roleDetails = _dataAccessAOP.GetAllRoles(userdetails.UserId);
                    string Roles = string.Empty;
                    for (int i = 0; i < roleDetails.Count; i++)
                    {
                        Roles += i > 0 ? (", " + roleDetails[i].Role) : roleDetails[i].Role;
                    }
                    string RefreshToken = GenerateRefreshToken();
                    bool logResponse = logUserSession(userdetails.UserId.ToString(), RefreshToken, "C");

                    if (logResponse)
                    {
                        List<Claim> claimsIn = new List<Claim>();

                        claimsIn.Add(new Claim("UserId", userdetails.UserId.ToString()));
                        claimsIn.Add(new Claim("FullName", userdetails.UserName));
                        claimsIn.Add(new Claim("Role", Roles));

                        JsonString = JsonConvert.SerializeObject(GenerateAccessToken(claimsIn, RefreshToken), Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }
                    else
                    {
                        JsonString = JsonConvert.SerializeObject(GenerateAccessToken(null, null), Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }

                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonString;
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Your email address and password not matched. Please try again with different email address or password.");
                }

            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
                _dataAccessAOP.LogErrorinDB("A", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            return responseMsg;
        }
        public bool logUserSession(string UserId, string RefreshToken, string Behaviour)
        {
            //Behaviour="C" for new, Behaviour="U" for refresh, Behaviour="L" for logout
            var result = _dataAccessAOP.logUserSession(UserId, RefreshToken, Behaviour);
            if (result == "s")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public MOAuth.jwtToken GenerateAccessToken(IEnumerable<Claim> claims, string RefreshToken)
        {
            MOAuth.jwtToken jwtTokenResult = new MOAuth.jwtToken();
            int TokenExpiryInMinute = Convert.ToInt32(POCO.AppSettings.TokenExpiryInMinute) * 60;

            if (claims != null)
            {
                jwtTokenResult.tokenIssuedAt = DateTime.UtcNow;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(POCO.AppSettings.Secret));

                var jwtToken = new JwtSecurityToken(issuer: POCO.AppSettings.Issuer,
                    audience: POCO.AppSettings.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddSeconds(TokenExpiryInMinute),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                jwtTokenResult.jwtaccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                jwtTokenResult.refreshToken = RefreshToken;
                jwtTokenResult.Role = claims.FirstOrDefault(c => c.Type == "Role").Value;
                jwtTokenResult.UserId = claims.FirstOrDefault(c => c.Type == "UserId").Value;
                jwtTokenResult.FullName = claims.FirstOrDefault(c => c.Type == "FullName").Value;

            }
            else
            {
                jwtTokenResult.jwtaccessToken = "Invalid credentials!";
                jwtTokenResult.refreshToken = "Invalid credentials!";
            }
            return jwtTokenResult;
        }
        public Library.APIResponse Logout(HttpRequest httpContext)
        {
            try
            {
                string JsonString = string.Empty;
                Global global = new Global();
                var principal = GetPrincipalFromExpiredToken(httpContext.Headers["Authorization"].ToString().Replace("Bearer ", ""));
                //bool logResponse = logUserSession(global.ReadClaim(httpContext, "UserId"), httpContext.Request.Headers["refreshToken"], "L");

                bool logResponse = logUserSession(principal.Claims.FirstOrDefault(c => c.Type == "UserId").Value, httpContext.Headers["refreshToken"], "L");
                JsonString = JsonConvert.SerializeObject(logResponse, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                responseMsg.ResponseContent = JsonString;
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }
        public Library.APIResponse RefreshToken(HttpRequest Request)
        {
            try
            {
                string JsonString = string.Empty;
                var principal = GetPrincipalFromExpiredToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

                //log into DB
                bool logResponse = logUserSession(principal.Claims.FirstOrDefault(c => c.Type == "UserId").Value, Request.Headers["refreshToken"], "U");

                if (logResponse)
                {
                    JsonString = JsonConvert.SerializeObject(GenerateAccessToken(principal.Claims, Request.Headers["refreshToken"]), Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                {
                    JsonString = JsonConvert.SerializeObject(GenerateAccessToken(null, null), Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                responseMsg.ResponseContent = JsonString;
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(POCO.AppSettings.Secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = new ClaimsPrincipal();
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return principal;
        }
    }
}
