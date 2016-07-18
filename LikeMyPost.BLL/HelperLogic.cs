using ILikeMyPost.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;
using ILikeMyPost.DAL;
using LikeMyPost.DAL;
using DAL.Exceptions;

namespace LikeMyPost.BLL
{
    public class HelperLogic : IHelperBLL
    {
        private IHelperDAL DAL;

        public HelperLogic()
        {
            DAL = new HelperDA();
        }
        public bool AddUser(string username, string email, byte[] password)
        {
            if (!IsUserExist(username, email))
            {
                return DAL.AddUser(username, email, password);
            }
            else
            {
                return false;
            }
        }

        private bool IsUserExist(string username, string mail)
        {
            return DAL.IsUserExist(username, mail);
        }

        public UserDTO GetUserByMail(string email)
        {
            return DAL.GetUserByMail(email);
        }
        public UserDTO GetUserByName(string name)
        {
            return DAL.GetUserByName(name);
        }
        public UserDTO GetUserById(int id)
        {
            return DAL.GetUserById(id);
        }

        public List<UserDTO> GetAllUsers(bool moderators)
        {
            try
            {
                if (moderators)
                {
                    return DAL.GetAllUsers();
                }
                else
                {
                    return DAL.GetPoorUsers();
                }
            }
            catch(DataAccessException e)
            {
                throw e;
            }
        }
    }
}
