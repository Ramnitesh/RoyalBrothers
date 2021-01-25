﻿using RoleBasedAccessControl.Model;
using RoleBasedAccessControl.Models;
using RoleBasedAccessControl.POCO;
using RoleBasedAccessControl.Queries.User.Interfaces;
using Lib.Databse.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Queries.User.Implementation
{
    public partial class UserDataAccess : IUserDataAccess
    {
        public async Task<bool> AddNewUser(string password, MUsers.MUsersDetails userdetails)
        {
            var dbContextTransaction = _AOnePageDBContext.Database.BeginTransaction();
            try
            {
                if (_AOnePageDBContext != null)
                {
                    TblUserMaster tblUserMaster = new TblUserMaster();
                    tblUserMaster.FullName = userdetails.FullName;
                    tblUserMaster.Password = password;
                    tblUserMaster.PersonalMailId = userdetails.PersonalMailId;
                    await _AOnePageDBContext.TblUserMasters.AddAsync(tblUserMaster);
                    await _AOnePageDBContext.SaveChangesAsync();

                    int userid = await _AOnePageDBContext.TblUserMasters.Where(x => x.FullName == userdetails.FullName && x.PersonalMailId == x.PersonalMailId).Select(x=>x.UserId).FirstOrDefaultAsync();

                    foreach (MUsers.MRole role in userdetails.Roles)
                    {
                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.UserId = userid;
                        tblUserRole.RoleId = role.RoleId;
                        await _AOnePageDBContext.TblUserRoles.AddAsync(tblUserRole);
                    }
                    await _AOnePageDBContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
            return true;
        }

        public async Task<List<MUsers.MUsersDetails>> GetUserDetails(int userid)
        {
            var dbContextTransaction = _AOnePageDBContext.Database.BeginTransaction();
            List<MUsers.MUsersDetails> mUsersDetailsList = new List<MUsers.MUsersDetails>();
            try
            {
                if (_AOnePageDBContext != null)
                {
                    if (true)
                    {
                        mUsersDetailsList = await _AOnePageDBContext.TblUserMasters.Where(x => userid != 0 ? x.UserId == userid : true).Select(x => new MUsers.MUsersDetails {
                            UserId = x.UserId,
                            FullName = x.FullName,
                            PersonalMailId = x.PersonalMailId,
                            Password = x.Password,
                            Roles = x.TblUserRoles.Where(r => r.UserId == x.UserId).Select(ru => 
                            new MUsers.MRole { UserId = ru.UserId, RoleId = ru.RoleId, UroleId = ru.UroleId, RoleName=_AOnePageDBContext.TblRoleMasters.Where(r=>r.RoleId==ru.RoleId).Select(r=> r.Role).FirstOrDefault().ToString() }).ToList()
                        }).ToListAsync();
                    }
                    dbContextTransaction.Commit();
                    return mUsersDetailsList;
                }
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
            return mUsersDetailsList;
        }

        public async Task<bool> UpdateUserDetails(MUsers.MUsersDetails userdetails)
        {
            var dbContextTransaction = _AOnePageDBContext.Database.BeginTransaction();
            try
            {
                if (_AOnePageDBContext != null)
                {
                    List<TblUserRole> mRoles = await _AOnePageDBContext.TblUserRoles.Where(x => x.UserId == userdetails.UserId).ToListAsync();
                    foreach (var userRole in mRoles)
                    {
                        _AOnePageDBContext.TblUserRoles.Remove(userRole);
                    }
                    await _AOnePageDBContext.SaveChangesAsync();

                    TblUserMaster tblUserMaster = await _AOnePageDBContext.TblUserMasters.Where(x => x.UserId == userdetails.UserId).FirstOrDefaultAsync();
                    tblUserMaster.FullName = userdetails.FullName;
                    tblUserMaster.Password = userdetails.Password;
                    //tblUserMaster.PersonalMailId = userdetails.PersonalMailId;
                    _AOnePageDBContext.TblUserMasters.Update(tblUserMaster);

                    foreach (MUsers.MRole role in userdetails.Roles)
                    {
                        TblUserRole tblUserRole = new TblUserRole();
                        tblUserRole.UserId = tblUserMaster.UserId;
                        tblUserRole.RoleId = role.RoleId;
                        await _AOnePageDBContext.TblUserRoles.AddAsync(tblUserRole);
                    }
                    await _AOnePageDBContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
            return true;
        }
        public async Task<bool> DeleteUserDetails(int UserId)
        {
            var dbContextTransaction = _AOnePageDBContext.Database.BeginTransaction();
            try
            {
                if (_AOnePageDBContext != null)
                {
                    List<TblUserRole> mRoles = await _AOnePageDBContext.TblUserRoles.Where(x => x.UserId == UserId).ToListAsync();
                    foreach (var userRole in mRoles)
                    {
                        _AOnePageDBContext.TblUserRoles.Remove(userRole);
                    }
                    TblUserMaster tblUserMaster = await _AOnePageDBContext.TblUserMasters.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
                    _AOnePageDBContext.TblUserMasters.Remove(tblUserMaster);

                    await _AOnePageDBContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
            return true;
        }
    }
}