using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.BLL
{
    public interface IHelperBLL
    {
        UserDTO GetUserByMail(string email);
        UserDTO GetUserByName(string name);
        UserDTO GetUserById(int id);
        List<UserDTO> GetAllUsers(bool moderators);
        bool AddUser(string username, string email, byte[] password);
    }
}
