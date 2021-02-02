using RoleBasedAccessControl.Queries.User.Interfaces;
using RoleBasedAccessControl.Model;
using Lib.Databse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoleBasedAccessControl.POCO;
using RoleBasedAccessControl.Models;

namespace RoleBasedAccessControl.Queries.User.Implementation
{
    public partial class UserDataAccess : IUserDataAccess
    {
        private readonly rbrbacdbContext _AOnePageDBContext;
        public UserDataAccess(rbrbacdbContext Context)
        {
            _AOnePageDBContext = Context;
        }

        public rbrbacdbContext getContext()
        {
            return _AOnePageDBContext;
        }
        #region Error Log
        public async void LogErrorinDB(string ErrorType, string ErrorSourceDetails, string ErrorDescription)
        {
            try
            {
                if (_AOnePageDBContext != null)
                {
                    TblErrorLogs errorLogs = new TblErrorLogs();
                    errorLogs.ErrorType = ErrorType;
                    errorLogs.ErrorSourceDetails = ErrorSourceDetails;
                    errorLogs.ErrorDescription = ErrorDescription;
                    errorLogs.LoggedDateTime = Library.UtcDateTime();
                    await _AOnePageDBContext.AddAsync(errorLogs);
                    await _AOnePageDBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
