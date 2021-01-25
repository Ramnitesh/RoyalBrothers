using Newtonsoft.Json;
using RoleBasedAccessControl.Interface;
using RoleBasedAccessControl.Model;
using RoleBasedAccessControl.Models;
using RoleBasedAccessControl.Queries.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Repository
{
    public class RUsers : IUsers
    {
        private readonly Library.APIResponse responseMsg;
        private Library.APIResponse _response = new Library.APIResponse();
        private IUserDataAccess _iUserDataAccess;
        public RUsers(IUserDataAccess iUserDataAccess)
        {
            _iUserDataAccess = iUserDataAccess;
            responseMsg = new Library.APIResponse();
        }
        public async Task<Library.APIResponse> AddNewUser(string password, MUsers.MUsersDetails userdetails)
        {
            try
            {
                bool user = await _iUserDataAccess.AddNewUser(password, userdetails);
                if (user)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("User added successfully.");
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Something went wrong while processing your request.");
                }
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }

        public async Task<Library.APIResponse> GetUserDetails(int userid)
        {
            try
            {
                List<MUsers.MUsersDetails> mUsersList = await _iUserDataAccess.GetUserDetails(userid);
                if (mUsersList != null)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject(mUsersList);
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Something went wrong while processing your request.");
                }
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }

        public async Task<Library.APIResponse> UpdateUserDetails(MUsers.MUsersDetails userdetails)
        {
            try
            {
                bool user = await _iUserDataAccess.UpdateUserDetails(userdetails);
                if (user)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("User details updated successfully.");
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Something went wrong while processing your request.");
                }
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }
        public async Task<Library.APIResponse> DeleteUserDetails(int UserId)
        {
            try
            {
                bool user = await _iUserDataAccess.DeleteUserDetails(UserId);
                if (user)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("User details deleted successfully.");
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Something went wrong while processing your request.");
                }
            }
            catch (Exception)
            {
                responseMsg.StatusCode = 500;
                responseMsg.ResponseContent = JsonConvert.SerializeObject(new Library.ExceptionResponse());
            }
            return responseMsg;
        }
    }
}
