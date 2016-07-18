using ILikeMyPost.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;
using System.Configuration;
using System.Data.SqlClient;
using NLog;
using DAL.Exceptions;

namespace LikeMyPost.DAL
{
    public class HelperDA : IHelperDAL
    {
        public HelperDA()
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public bool AddUser(string username, string email, byte[] password)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.Users (username, mail, password) VALUES(@Username, @Email, @Password) ";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't add new user to db");
                }
            }
            return summary > 0;
        }

        public bool IsUserExist(string username, string mail)
        {
            int users = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id FROM Users WHERE username = @UserName OR mail = @Mail";
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Mail", mail);
                    connection.Open();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users++;
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get user");
                }
                return users > 0;
            }
        }

        public UserDTO GetUserById(int id)
        {
            using (var connection = new SqlConnection(CS))
            {

                var command = connection.CreateCommand();
                command.CommandText = "SELECT mail, username, password, raiting FROM dbo.Users WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                UserDTO user = null;
                try {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string email = (string)reader["mail"];
                            int raiting = (int)reader["raiting"];
                            string name = (string)reader["username"];
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

        public UserDTO GetUserByName(string name)
        {
            using (var connection = new SqlConnection(CS))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, mail, password, raiting FROM dbo.Users WHERE username = @Name";
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                UserDTO user = null;
                try {
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

        public UserDTO GetUserByMail(string email)
        {
            using (var connection = new SqlConnection(CS))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, username, password, raiting FROM dbo.Users WHERE mail = @Email";
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                UserDTO user = null;
                try {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            string name = (string)reader["username"];
                            byte[] password = (byte[])reader["password"];
                            int raiting = (int)reader["raiting"];
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

        public bool Raiting(int user_id, int raiting)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Users SET  raiting = raiting + @Raiting  WHERE id = @UserID ";
                    command.Parameters.AddWithValue("@UserID", user_id);
                    command.Parameters.AddWithValue("@Raiting", raiting);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't change raiting");
                }
            }
            return summary > 0;
        }

        public List<UserDTO> GetAllUsers()
        {
            using (var connection = new SqlConnection(CS))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, mail, username, password, raiting FROM dbo.Users";
                connection.Open();
                List<UserDTO> users = new List<UserDTO>();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int user_id = (int)reader["id"];
                            string email = (string)reader["mail"];
                            int raiting = (int)reader["raiting"];
                            string name = (string)reader["username"];
                            byte[] password = (byte[])reader["password"];
                            users.Add(new UserDTO(user_id, name, email, password, raiting));
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get users from database. Try find some problems here =)");
                }
                return users;
            }
        }

        public List<UserDTO> GetPoorUsers()
        {
            using (var connection = new SqlConnection(CS))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, mail, username, password, raiting FROM dbo.Users WHERE id NOT IN (SELECT id FROM dbo.Users as u, UsersInRoles as ur WHERE u.id = ur.user_id  AND (ur.role_id = 3 OR ur.role_id = 1))";
                connection.Open();
                List<UserDTO> users = new List<UserDTO>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            int user_id = (int)reader["id"];
                            string email = (string)reader["mail"];
                            int raiting = (int)reader["raiting"];
                            string name = (string)reader["username"];
                            byte[] password = (byte[])reader["password"];
                            users.Add(new UserDTO(user_id, name, email, password, raiting));
                        }
                    }
                    catch
                    {
                        throw new DataAccessException("Can't get users from database. Try find some problems here =)");
                    }
                }
                return users;
            }
        }
    }
}
