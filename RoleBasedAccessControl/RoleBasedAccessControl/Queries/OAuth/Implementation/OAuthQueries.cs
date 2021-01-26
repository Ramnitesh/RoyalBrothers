using Lib.Databse.Models;
using Microsoft.EntityFrameworkCore;
using RoleBasedAccessControl.Models;
using RoleBasedAccessControl.Queries.OAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MOAuth;

namespace RoleBasedAccessControl.Queries.OAuth.Implementation
{
    public partial class OAuthDataAccess : IOAuthDataAccess
    {
        public string GetConnectionString(string TenantGuID)
        {
            try
            {
                if (_RBContext != null)
                {
                    var query = (from CM in _RBContext.TblUserMasters
                                 select CM.Password
                                 );
                    return query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public List<MOAuth.Roles> GetAllRoles(int userid)
        {
            List<MOAuth.Roles> roles = new List<MOAuth.Roles>();
            try
            {
                if (_RBContext != null)
                {
                    roles =  (from TRM in _RBContext.TblRoleMasters
                                   join UR in _RBContext.TblUserRoles on TRM.RoleId equals UR.RoleId
                                   where UR.UserId == userid && UR.RoleId != 5
                                   select new MOAuth.Roles
                                   {
                                       RoleId = TRM.RoleId,
                                       Role = TRM.Role
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }
        public string logUserSession(string UserId, string RefreshToken, string Behaviour)
        {
            var dbContextTransaction = _RBContext.Database.BeginTransaction();
            try
            {
                if (Behaviour == "C")
                {
                    TblUserSession tblUserSession = new TblUserSession()
                    {
                        SessionGuid = System.Guid.NewGuid().ToString(),
                        RefreshToken = RefreshToken,
                        Userid = Convert.ToInt32(UserId),
                        StartedAt = DateTime.UtcNow,
                        RefreshedAt = null,
                        EndedAt = null
                    };
                    _RBContext.AddAsync(tblUserSession);
                    dbContextTransaction.Commit();
                    _RBContext.SaveChangesAsync();
                    return "s";
                }
                else
                {
                    var ifexist = _RBContext.TblUserSessions.Where(p => p.Userid == Convert.ToInt32(UserId) && p.RefreshToken == RefreshToken).Select(p => p.SessionGuid).FirstOrDefault();
                    if (ifexist != null)
                    {
                        if (Behaviour == "U")
                        {
                            TblUserSession tblUserSession = _RBContext.TblUserSessions.Where(p => p.Userid == Convert.ToInt32(UserId) && p.RefreshToken == RefreshToken).FirstOrDefault();
                            tblUserSession.RefreshedAt = DateTime.UtcNow;
                            _RBContext.Update(tblUserSession);
                        }
                        else if (Behaviour == "L")
                        {
                            TblUserSession tblUserSession = _RBContext.TblUserSessions.Where(p => p.Userid == Convert.ToInt32(UserId) && p.RefreshToken == RefreshToken).FirstOrDefault();
                            tblUserSession.EndedAt = DateTime.UtcNow;
                            _RBContext.Update(tblUserSession);
                        }
                        dbContextTransaction.Commit();
                        _RBContext.SaveChangesAsync();
                        return "s";
                    }
                    else
                    {
                        return "f";
                    }
                }
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
        }
        public async Task<string> VerifyAuthenticationCode(string emailId, string privacyCode)
        {
            try
            {
                if (_RBContext != null)
                {
                    TblUserMaster tblUserMaster = new TblUserMaster();
                    tblUserMaster = await _RBContext.TblUserMasters.Where(p => p.PersonalMailId == emailId && p.IsLoginDisabled == false && p.IsUserDisabled == false && p.PrivacyCode == privacyCode).SingleOrDefaultAsync();
                    if (tblUserMaster != null)
                    {
                        return "s";
                    }
                    else
                    {
                        return "n";
                    }
                }
                return "n";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
        public LoginResponse ValidateUser(string username, string password, int roleId)
        {
            var dbContextTransaction = _RBContext.Database.BeginTransaction();
            try
            {
                if (_RBContext != null)
                {
                    var query = (from tbl_UM in _RBContext.TblUserMasters
                                       where tbl_UM.Password == password & tbl_UM.IsLoginDisabled == false & tbl_UM.IsUserDisabled == false & (tbl_UM.PersonalMailId == username)
                                       select new LoginResponse
                                       {
                                           UserId = tbl_UM.UserId,
                                           IsLoginDisabled = tbl_UM.IsLoginDisabled,
                                           IsUserDisabled = tbl_UM.IsUserDisabled,
                                           UserName = tbl_UM.PersonalMailId
                                       }).FirstOrDefault();
                    dbContextTransaction.Commit();
                    return query;
                }
                dbContextTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
            return new LoginResponse();
        }
        
        
    }
}
