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
        private readonly ramnitesh_aonepageContext _RBContext;
        public OAuthDataAccess(ramnitesh_aonepageContext Context)
        {
            _RBContext = Context;
        }
        public ramnitesh_aonepageContext getContext()
        {
            return _RBContext;
        }

    }
}
