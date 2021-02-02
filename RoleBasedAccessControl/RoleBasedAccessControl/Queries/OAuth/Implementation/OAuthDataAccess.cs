using RoleBasedAccessControl.Queries.OAuth.Interfaces;
using RoleBasedAccessControl.Model;
using Lib.Databse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Queries.OAuth.Implementation
{
    public partial class OAuthDataAccess : IOAuthDataAccess
    {
        private readonly rbrbacdbContext _RBContext;
        public OAuthDataAccess(rbrbacdbContext Context)
        {
            _RBContext = Context;
        }
        public rbrbacdbContext getContext()
        {
            return _RBContext;
        }
        #region Error Log
        public async void LogErrorinDB(string ErrorType, string ErrorSourceDetails, string ErrorDescription)
        {
            try
            {
                if (_RBContext != null)
                {
                    TblErrorLogs errorLogs = new TblErrorLogs();
                    errorLogs.ErrorType = ErrorType;
                    errorLogs.ErrorSourceDetails = ErrorSourceDetails;
                    errorLogs.ErrorDescription = ErrorDescription;
                    errorLogs.LoggedDateTime = Library.UtcDateTime();
                    await _RBContext.AddAsync(errorLogs);
                    await _RBContext.SaveChangesAsync();
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
