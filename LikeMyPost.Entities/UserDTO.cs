using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class UserDTOClone
    {
        public int Id { get; private set; }
        public int Raiting { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public int NumberOfPosts { get; private set; }
        public int NumberOfComments { get; private set; }
        public byte[] Password { get; private set; }

        //public UserDTO(int id, int raiting, string name, string mail, DateTime date, int posts, int comments, byte[] password) // Get User from BD
        //{
        //    Id = id;
        //    Raiting = raiting;
        //    Name = name;
        //    Mail = mail;
        //    RegisterDate = date;
        //    NumberOfPosts = posts;
        //    NumberOfComments = comments;
        //    Password = password;
        //}

        //public UserDTO(string name, string mail, byte[] password)  //Creating New User
        //{
        //    Raiting = 0;
        //    Name = name;
        //    Mail = mail;
        //    RegisterDate = DateTime.Now;
        //    NumberOfPosts = 0;
        //    NumberOfComments = 0;
        //    Password = password;
        //}
    }
}
