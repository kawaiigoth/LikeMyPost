using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class RoleDTO
    {
        public int Id { get; private set; }       
        public string roleName { get; private set; }
        public string Description { get; private set; }

        public RoleDTO(int id, string name, string desc) 
        {
            Id = id;
            roleName = name;
            Description = desc;
        }
        
    }
}
