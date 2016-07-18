using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.DAL
{
    public interface IRoleProviderDAL
    {
        bool AddRoleToUser(int user_id, int role_id);
        void RemoveRoleFromUser(int user_id, int role_id);
        UserDTO GetUserByName(string username);
        RoleDTO GetRoleByName(string roleName);
        string[] GetRolesForUser(int user_id);
        bool IsUserInRole(int user_id, int role_id);
    }
}
