using ILikeMyPost.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;
using DAL.Exceptions;

namespace LikeMyPost.DAL
{
    public class RoleProviderDA : IRoleProviderDAL
    {
        public RoleProviderDA()
        {
            try
            {
                CS = ConfigurationManager.ConnectionStrings["LikeMyPostDB"].ConnectionString;
            }
            catch
            {
                throw new ConfigurationErrorsException("Bad Connection String");
            }
        }
        string CS;

        public RoleDTO GetRoleByName(string roleName)
        {
            using (var connection = new SqlConnection(CS))
            {
                
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id,  description  FROM dbo.Roles WHERE roleName = @RoleName";
                command.Parameters.AddWithValue("@RoleName", roleName);
                connection.Open();
                RoleDTO Role = null;
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = (int)reader["id"];
                            var desc = (string)reader["description"];
                            Role = new RoleDTO(id, roleName, desc);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get role");
                }
                return Role;
            }
        }

        public bool AddRoleToUser(int user_id, int role_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.UsersInRoles (user_id, role_id) VALUES(@UserID, @RoleID) ";
                    command.Parameters.AddWithValue("@UserID", user_id);
                    command.Parameters.AddWithValue("@RoleID", role_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't add roles");
                }
            }
            return summary > 0;
        }

        public void RemoveRoleFromUser(int user_id, int role_id)
        {
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM dbo.UsersInRoles WHERE user_id= @UserID AND role_id = @RoleID ;";
                    command.Parameters.AddWithValue("@RoleID", role_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataAccessException("Can't remove roles");
                }
            }          
        }

        public string[] GetRolesForUser(int user_id)
        {
            List<string> roles = new List<string>();
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT r.roleName FROM Roles as r,  UsersInRoles ur WHERE ur.role_id = r.id AND ur.user_id = @ID";
                    command.Parameters.AddWithValue("@ID", user_id);
                    connection.Open();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var role = (string)reader["roleName"];
                            roles.Add(role);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get roles");
                }
                return roles.ToArray();
            }
        }

        public UserDTO GetUserByName(string name)
        {
            using (var connection = new SqlConnection(CS))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, mail, password, raiting FROM dbo.Users WHERE username = @Name";
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                UserDTO user = null;
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            int raiting = (int)reader["raiting"];
                            string email = (string)reader["mail"];
                            byte[] password = (byte[])reader["password"];
                            user = new UserDTO(id, name, email, password, raiting);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get user");
                }

                return user;
            }
        }

        public bool IsUserInRole(int user_id, int role_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT ur.user_id, ur.role_id FROM dbo.UsersInRoles ur WHERE ur.user_id=@UserID AND ur.role_id=@RoleID";
                    command.Parameters.AddWithValue("@UserID", user_id);
                    command.Parameters.AddWithValue("@RoleID", role_id);
                    connection.Open();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            summary++;

                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get user and role ID");
                }
                return summary > 0;


            }
        }

    }
}
