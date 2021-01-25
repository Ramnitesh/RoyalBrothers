using RoleBasedAccessControl.Queries.User.Interfaces;
using RoleBasedAccessControl.Model;
using Lib.Databse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoleBasedAccessControl.POCO;

namespace RoleBasedAccessControl.Queries.User.Implementation
{
    public partial class UserDataAccess : IUserDataAccess
    {
        private readonly ramnitesh_aonepageContext _AOnePageDBContext;
        public UserDataAccess(ramnitesh_aonepageContext Context)
        {
            _AOnePageDBContext = Context;
        }

        public ramnitesh_aonepageContext getContext()
        {
            return _AOnePageDBContext;
        }
    }
}
