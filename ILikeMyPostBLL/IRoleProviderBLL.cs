using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.BLL
{
    public interface IRoleProviderBLL
    {
        bool RemoveRoleFromUser(int user_id, string roleName);
        bool AddUserToRole(int user_id, string roleName);
        string[] GetRolesForUser(string username);
        bool IsUserInRole(string username, string roleName);
    }
}
