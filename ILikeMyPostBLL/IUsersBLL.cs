using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.BLL
{
    public interface IUsersBLL
    {
        bool AddUser(UserDTO user);
        //bool BanUser(int user_id);
        //UserDTO GetUserByMail(string email);
        //UserDTO GetUserById(int user_id);
        //UserDTO GetUserByName(string username);
        string[] GetRolesForUser(string username);
        string[] GetUsersInRole(string roleName);
        bool IsUserInRole(string username, string roleName);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        void AddUsersToRoles(string[] usernames, string[] roleNames);
    }
}
