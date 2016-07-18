using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class UserDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }        
        public byte[] Password { get; private set; }
        public int Raiting { get; private set; }

        public UserDTO(int id, string name, string mail, byte[] password, int raiting) // Get User from BD
        {
            Id = id;
            Name = name;
            Mail = mail;
            Password = password;
            Raiting = raiting;
        }
        
    }
}
