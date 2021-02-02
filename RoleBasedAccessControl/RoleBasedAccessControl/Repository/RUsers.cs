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

        public async Task<Library.APIResponse> AddRoleSourceActionDetails(MUsers.MRoleSourceAction mRoleSourceAction)
        {
            try
            {
                bool user = await _iUserDataAccess.AddRoleSourceActionDetails(mRoleSourceAction);
                if (user)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Role added successfully.");
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
        public async Task<Library.APIResponse> GetRoleSourceActionDetails(int userid, string role)
        {
            try
            {
                List<MUsers.MRoleSourceAction> roleList = await _iUserDataAccess.GetRoleSourceActionDetails(userid,role);
                if (roleList != null)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject(roleList);
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
        public async Task<Library.APIResponse> GetAllRolesOnly()
        {
            try
            {
                List<MUsers.MRole> roleList = await _iUserDataAccess.GetAllRolesOnly();
                if (roleList != null)
                {
                    responseMsg.StatusCode = 200;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject(roleList);
                }
                else
                {
                    responseMsg.StatusCode = 206;
                    responseMsg.ResponseContent = JsonConvert.SerializeObject("Something went wrong while processing your request.");
                }
            }
            catch (Exception ex)
            {
                responseMsg.StatusCode = 200;
                responseMsg.ResponseContent = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + "Error Description:- " + ex.ToString();// JsonConvert.SerializeObject(new Library.ExceptionResponse());
                _iUserDataAccess.LogErrorinDB("A", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            return responseMsg;
        }
    }
}
