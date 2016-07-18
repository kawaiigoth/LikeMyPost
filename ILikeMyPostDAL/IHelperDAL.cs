using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.DAL
{
    public interface IHelperDAL
    {
        bool Raiting(int user_id, int raiting);
        bool IsUserExist(string username, string mail);
        bool AddUser(string username, string email, byte[] password);
        UserDTO GetUserByMail(string email);
        UserDTO GetUserByName(string name);
        UserDTO GetUserById(int id);
        List<UserDTO> GetAllUsers();
        List<UserDTO> GetPoorUsers();
    }
}
