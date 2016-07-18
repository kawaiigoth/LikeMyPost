using DAL.Exceptions;
using ILikeMyPost.BLL;
using ILikeMyPost.DAL;
using LikeMyPost.DAL;
using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.BLL
{
    public class RoleProviderBLL : IRoleProviderBLL
    {
        private IRoleProviderDAL DAL;

        public RoleProviderBLL()
        {
            try {
                DAL = new RoleProviderDA();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public bool AddUserToRole(int user_id, string roleName)
        {
            RoleDTO role = null;
            try
            {
                role = DAL.GetRoleByName(roleName);
            }
            catch(DataAccessException e)
            {
                throw e;             
            }
            if (role != null)
            {
                try {
                    return DAL.AddRoleToUser(user_id, role.Id);
                }
                catch(DataInsertException e)
                {
                    throw e;
                }
            }
            else
            {
                return false;
            }

        }

        public string[] GetRolesForUser(string username)
        {
            UserDTO user = null;
            try
            {
                user = DAL.GetUserByName(username);
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            if(user !=null)
            {
                try
                {
                    return DAL.GetRolesForUser(user.Id);
                }
                catch(DataAccessException e)
                {
                    throw e;
                }
            }
            else
            {
                return null;
            }
            
        }

        public bool IsUserInRole(string username, string roleName)
        {
            UserDTO user = null;
            try
            {
                user = DAL.GetUserByName(username);
            }
             catch(DataAccessException e)
            {
                throw e;
            }

            RoleDTO role = null;
            try
            {
                role = DAL.GetRoleByName(roleName);
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            if(role != null && user != null)
            {
                try
                {
                    return DAL.IsUserInRole(user.Id, role.Id);
                }
                catch(DataAccessException e)
                { throw e; }
            }
            else return false;
        }

        public bool RemoveRoleFromUser(int user_id, string roleName)
        {
            RoleDTO role = null;
            try
            {
                role = DAL.GetRoleByName(roleName);
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            if(role != null)
            {
                try
                {
                    DAL.RemoveRoleFromUser(user_id, role.Id);
                    return true;
                }
                catch(DataAccessException e)
                {
                    throw e;
                }
            }
            else
            {
                return false;
            }
            
            
        }
    }
}
