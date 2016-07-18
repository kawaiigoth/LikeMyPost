using ILikeMyPostDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;
using System.Configuration;
using System.Data.SqlClient;
using DAL.Exceptions;

namespace LikeMyPost.DAL
{
    public class PostDA : IPostDAL
    {
        public PostDA()
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

        public bool AddPost(PostDTO post)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.Posts (author, heading, image_id, text, date) VALUES(@PostAuthor, @PostHeading, @PostImage, @PostText, @PostDate) ";
                    command.Parameters.AddWithValue("@PostAuthor", post.Author);
                    command.Parameters.AddWithValue("@PostHeading", post.Heading);
                    if (post.Image != Guid.Empty)
                    {
                        command.Parameters.AddWithValue("@PostImage", post.Image);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostImage", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@PostText", post.Text);
                    command.Parameters.AddWithValue("@PostDate", post.Date);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Posts insert Error");
                }
            }
            return summary > 0;
        }

        public List<PostDTO> GetPosts()
        {
            List<PostDTO> posts = new List<PostDTO>();
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, author, heading, image_id, text, date, raiting FROM dbo.Posts WHERE isBaned = 0";
                    connection.Open();
                   
                    string author;
                    int post_id;
                    Guid image_id;
                    DateTime date;
                    string heading;
                    string text;
                    object temp;
                    int raiting;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            post_id = (int)reader["id"];
                            author = (string)reader["author"];
                            if (DBNull.Value != (temp = reader["image_id"]))
                            {
                                image_id = (Guid)temp;
                            }
                            else
                            {
                                image_id = Guid.Empty;
                            }
                            date = (DateTime)reader["date"];
                            heading = (string)reader["heading"];
                            text = (string)reader["text"];
                            raiting = (int)reader["raiting"];
                            posts.Add(new PostDTO(post_id, author, date, heading, text, image_id, raiting));
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Posts select error");
                }
                return posts;
            }
        }

        public PostDTO GetPostByID(int id)
        {
            PostDTO post = null;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT author, heading, image_id, text, date, raiting FROM dbo.Posts WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    string author;
                    Guid image_id;
                    DateTime date;
                    string heading;
                    string text;
                    int raiting;
                    object temp;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            author = (string)reader["author"];
                            if (DBNull.Value != (temp = reader["image_id"]))
                            {
                                image_id = (Guid)temp;
                            }
                            else
                            {
                                image_id = Guid.Empty;
                            }
                            date = (DateTime)reader["date"];
                            heading = (string)reader["heading"];
                            text = (string)reader["text"];
                            raiting = (int)reader["raiting"];
                            post = new PostDTO(id, author, date, heading, text, image_id, raiting);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Posts select error");
                }
                return post;
            }
        }

        public bool AddComment(CommentDTO comment)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.Comments (author,post_id, text, date) VALUES(@CommentAuthor, @PostId, @CommentText, @CommentDate) ";
                    command.Parameters.AddWithValue("@CommentAuthor", comment.Author);
                    command.Parameters.AddWithValue("@PostId", comment.Post_Id);
                    command.Parameters.AddWithValue("@CommentText", comment.Text);
                    command.Parameters.AddWithValue("@CommentDate", comment.Date);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Comments Insert Error");
                }
            }
            return summary > 0;
        }

        public List<CommentDTO> GetComments(int post_id)
        {
            List<CommentDTO> comments = new List<CommentDTO>();
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, author, text, date, raiting FROM dbo.Comments WHERE post_id = @PostID";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    connection.Open();

                    string author;
                    int id;
                    DateTime date;
                    string text;
                    int raiting;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = (int)reader["id"];
                            author = (string)reader["author"];
                            date = (DateTime)reader["date"];
                            text = (string)reader["text"];
                            raiting = (int)reader["raiting"];
                            comments.Add(new CommentDTO(id, author, text, post_id, date, raiting));
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Comments select Error");
                }
                return comments;
            }
        }

        public CommentDTO GetCommentByID(int id)
        {
            CommentDTO comment = null;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT author, text, date, raiting, post_id FROM dbo.Comments WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    string author;
                    DateTime date;
                    string text;
                    int raiting;
                    int post_id;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            author = (string)reader["author"];
                            post_id = (int)reader["post_id"];
                            date = (DateTime)reader["date"];
                            raiting = (int)reader["raiting"];
                            text = (string)reader["text"];
                            comment = new CommentDTO(id, author, text, post_id, date, raiting);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Comment select error");
                }
                return comment;
            }
        }

        public bool PostVote(int post_id, int user_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Posts SET  raiting = raiting + 1  WHERE id = @PostID ";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't add raiting to post");
                }
            }

            if (summary > 0)
            {
                using (var connection = new SqlConnection(CS))
                {
                    try
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO dbo.VotesPosts  (user_id, post_id) VALUES (@UserID, @PostID)   ";
                        command.Parameters.AddWithValue("@PostID", post_id);
                        command.Parameters.AddWithValue("@UserID", user_id);
                        connection.Open();
                        summary = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new DataInsertException("VotesPosts Insert Error");
                    }
                }
                return summary > 0;
            }

            else
            {
                return false;
            }
        }

        public bool CommentVote(int comment_id, int user_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Comments SET  raiting = raiting + 1  WHERE id = @CommentID ";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Comments raiting update error");
                }
            }

            if (summary > 0)
            {
                using (var connection = new SqlConnection(CS))
                {
                    try {
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO dbo.VotesComments  (user_id, comment_id) VALUES (@UserID, @CommentID)   ";
                        command.Parameters.AddWithValue("@CommentID", comment_id);
                        command.Parameters.AddWithValue("@UserID", user_id);
                        connection.Open();
                        summary = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new DataAccessException("VotesComments Insert Error");
                    }
                }
                return summary > 0;
            }

            else
            {
                return false;
            }
        }

        public bool PostLike(int post_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Posts SET  raiting = raiting + 1  WHERE id = @PostID ";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Posts raiting error");
                }
            }
            return summary > 0;
        }

        public bool PostDislike(int post_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Posts SET  raiting = raiting - 1  WHERE id = @PostID ";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Posts raiting error");
                }
            }
            return summary > 0;
        }

        public bool IsUserPostLike(int user_id, int post_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT user_id, post_id FROM dbo.VotesPosts WHERE user_id = @UserID AND post_id = @PostID";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
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
                    throw new DataAccessException("VotesPosts select error");
                }
                return summary > 0;
            }
        }

        public void RemoveUserPostLike(int user_id, int post_id)
        {
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM dbo.VotesPosts WHERE user_id= @UserID AND post_id = @PostID ;";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("VotesPosts delete error");
                }
            }

        }

        public void AddUserPostLike(int user_id, int post_id)
        {
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.VotesPosts (user_id, post_id) VALUES(@UserID, @PostID) ";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("VotesPosts insert error");
                }
            }
        }

        public bool CommentLike(int comment_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Comments SET  raiting = raiting + 1  WHERE id = @CommentID ";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Cant update comments");
                }
            }
            return summary > 0;
        }

        public bool CommentDislike(int comment_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Comments SET  raiting = raiting - 1  WHERE id = @CommentID ";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't update comments");
                }
            }
            return summary > 0;
        }

        public bool IsUserCommentLike(int user_id, int comment_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT user_id, comment_id FROM dbo.VotesComments WHERE user_id = @UserID AND comment_id = @CommentID";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
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
                    throw new DataAccessException("VotesComments select error");
                }
                return summary > 0;
            }
        }

        public void RemoveUserCommentLike(int user_id, int comment_id)
        {
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM dbo.VotesComments WHERE user_id= @UserID AND comment_id = @CommentID ;";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataAccessException("VotesComments delete err");
                }
            }
        }

        public void AddUserCommentLike(int user_id, int comment_id)
        {
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.VotesComments (user_id, comment_id) VALUES(@UserID, @CommentID) ";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    command.Parameters.AddWithValue("@UserID", user_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("VotesComments insert error");
                }
            }
        }

        public bool DeletePost(int post_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Posts SET  isBaned = 1  WHERE id = @PostID ";
                    command.Parameters.AddWithValue("@PostID", post_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't update post");
                }
            }
            return summary > 0;
        }

        public bool DeleteComment(int comment_id)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE dbo.Comments SET  isBaned = 1  WHERE id = @CommentID ";
                    command.Parameters.AddWithValue("@CommentID", comment_id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't update comment");
                }
            }
            return summary > 0;
        }
    }
}
