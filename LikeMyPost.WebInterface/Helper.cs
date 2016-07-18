using ILikeMyPost.BLL;
using ILikeMyPostBLL;
using LikeMyPost.BLL;
using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace LikeMyPost.WebInterface
{
    public class Helper
    {
        public static byte[] Crypt(string password)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            return provider.ComputeHash(encoding.GetBytes(password));
        }

        public static bool AddUserToRole(int user_id, string roleName)
        {
            IHelperBLL logic = new HelperLogic();
            IRoleProviderBLL role_logic = new RoleProviderBLL();
            var user = logic.GetUserById(user_id);
            if (!UserIsInRole(user.Name,roleName))
            {
                return role_logic.AddUserToRole(user_id, roleName);
            }
            else
            {
                return false;
            }
            
        }

        public static bool RemoveRoleFromUser(int user_id, string roleName)
        {
            IHelperBLL logic = new HelperLogic();
            IRoleProviderBLL role_logic = new RoleProviderBLL();
            var user = logic.GetUserById(user_id);
            if (UserIsInRole(user.Name, roleName))
            {
                return role_logic.RemoveRoleFromUser(user_id, roleName);
            }
            else
            {
                return false;
            }
        }

        public static int CanLogin(string email, string pass)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(pass))
            {
                IHelperBLL logic = new HelperLogic();
                UserDTO user = logic.GetUserByMail(email);
                if (user != null && user.Password.SequenceEqual(Crypt(pass)))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static int CanRegister(string email, string pass, string username)
        {
            int plht = pass.Length;
            int ulht = username.Length;
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(pass) && plht <= 12 && plht >= 6 && !string.IsNullOrWhiteSpace(username) && ulht <= 30 && ulht >= 4)
            {
                IHelperBLL logic = new HelperLogic();
                UserDTO user = logic.GetUserByMail(email);
                if(logic.AddUser(username, email, Crypt(pass)))
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        public static List<UserDTO> GetAllUsers(bool modrators)
        {
            try
            {
                IHelperBLL logic = new HelperLogic();
                return logic.GetAllUsers(modrators);
            }
            catch
            {
                throw;
            }
        }

        public static bool UserIsInRole(string username, string roleName)
        {
            IRoleProviderBLL logic = new RoleProviderBLL();
            return logic.IsUserInRole(username, roleName);
        }

        public static string GetUserNameByMail(string email)
        {
            IHelperBLL logic = new HelperLogic();
            var name = logic.GetUserByMail(email).Name;
            return name;
        }

        public static List<List<PostDTO>> PagesList(int postsOnAPage, List<PostDTO> posts)
        {
            int maxpages = (posts.Count > postsOnAPage) ? (posts.Count + postsOnAPage - 1) / postsOnAPage : 1;
            var PageList = new List<List<PostDTO>>(maxpages);
            for(int i = 0; i<maxpages; i += 1)
            {
                var elems = posts.Skip(i*postsOnAPage).Take(postsOnAPage);
                PageList.Add(new List<PostDTO>(elems));
            }
            return PageList;
        }

        public static List<PostDTO> GetNewPosts()
        {
            IPostBLL logic = new PostLogic();
            return logic.GetNewPosts();
        }


        public static List<PostDTO> GetBestPosts()
        {
            IPostBLL logic = new PostLogic();
            return logic.GetBestPosts();
        }

        public static List<PostDTO> GetHotPosts()
        {
            IPostBLL logic = new PostLogic();
            return logic.GetHotPosts();
        }

        public static int AddComment(string text, string author, int post_id)
        {
            IPostBLL logic = new PostLogic();
            if (text.Length > 500)
            {
                return 1;
            }
            CommentDTO comment = new CommentDTO(author, text, post_id);
            return logic.AddComment(comment);
        }

        public static List<CommentDTO> GetComments(int post_id)
        {
            IPostBLL logic = new PostLogic();
            return logic.GetComments(post_id);
        }

        public static int AddPost(string heading, WebImage img, string text, string author) //1 - ok, 0 -server validation fail, -1 - serverside error
        {
            int hlht = heading.Length;
            int tlht = text.Length;
            if (ImageCheck(img) && hlht > 2 && hlht <= 100 && !string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(author))
            {
                IPostBLL logic = new PostLogic();
                if(img.Width >= 700)
                {
                    img.Resize(700, img.Height, true);
                }
                ImageDTO image = new ImageDTO(img.ImageFormat, img.GetBytes());
                PostDTO post = new PostDTO(heading, image.Id, text, author);
                if (logic.AddPost(post, image))
                {
                    return 1;
                }
                return -1;
            }
            else
            {
                return 0;
            }

        }

        private static bool ImageCheck(WebImage img)
        {
            if (img.ImageFormat == "jpg" || img.ImageFormat == "jpeg" || img.ImageFormat == "gif" || img.ImageFormat == "png")
            {
                var img_bytes = img.GetBytes().Length;
                var img_weight = (img_bytes / 1024f) / 1024f;
                return img_weight <= 4f;
            }
            else
            {
                return false;
            }
        }

        public static int AddPost(string heading, string text, string author)
        {
            int hlht = heading.Length;
            int tlht = text.Length;
            if (hlht > 2 && hlht <= 100 && !string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(author))
            {
                IPostBLL logic = new PostLogic();
                PostDTO post = new PostDTO(heading, text, author);
                if (logic.AddPost(post, null))
                {
                    return 1;
                }
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public static PostDTO GetPostByID(int id)
        {
            IPostBLL logic = new PostLogic();
            PostDTO post = logic.GetPostByID(id);
            return post;
        }

        public static CommentDTO GetCommentById(int id)
        {
            IPostBLL logic = new PostLogic();
            CommentDTO comment = logic.GetCommentByID(id);
            return comment;
        }

        public static ImageDTO GetPostImage(Guid id)
        {
            IPostBLL logic = new PostLogic();
            ImageDTO image = logic.GetImage(id);
            return image;
        }


        public static bool IsVoted(string username, int post_id)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL post_logic = new PostLogic();
            var user = logic.GetUserByName(username);
            if (!string.IsNullOrWhiteSpace(username))
            {
                return post_logic.IsUserPostLike(post_id, user.Id);
            }
            else
            {
                return false;
            }
        }

        public static bool IsCommentVoted(string username, int comment_id)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL comm_logic = new PostLogic();
            var user = logic.GetUserByName(username);
            if (!string.IsNullOrWhiteSpace(username))
            {
                return comm_logic.IsUserCommentLike(comment_id, user.Id);
            }
            else
            {
                return false;
            }
        }

        public static bool CommentLike(int comment_id, string username)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL comment_logic = new PostLogic();
            var user = logic.GetUserByName(username);
            if (!string.IsNullOrWhiteSpace(username))
            {
                return comment_logic.CommentLike(comment_id, user.Id);
            }
            else
            {
                return false;
            }
        }

        public static bool PostLike(int post_id, string username)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL post_logic = new PostLogic();
            var user = logic.GetUserByName(username);
            if (!string.IsNullOrWhiteSpace(username))
            {
                return post_logic.PostLike(post_id, user.Id);
            }
            else
            {
                return false;
            }
        }

        public static bool DeletePost(int post_id)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL post_logic = new PostLogic();
            var post = post_logic.GetPostByID(post_id);
            var author = logic.GetUserByName(post.Author);
            return post_logic.DeletePost(post, author);
        }
        public static bool DeleteComment(int comment_id)
        {
            IHelperBLL logic = new HelperLogic();
            IPostBLL post_logic = new PostLogic();
            var comment = post_logic.GetCommentByID(comment_id);
            var author = logic.GetUserByName(comment.Author);
            return post_logic.DeleteComment(comment, author);
        }

    }
}