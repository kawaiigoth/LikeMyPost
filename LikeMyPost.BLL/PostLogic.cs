using ILikeMyPostBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;
using ILikeMyPostDAL;
using LikeMyPost.DAL;
using ILikeMyPost.DAL;
using DAL.Exceptions;

namespace LikeMyPost.BLL
{
    public class PostLogic : IPostBLL
    {
        private IPostDAL Post_DAL;
        private IImageDAL Image_DAL;
        private IHelperDAL Helper_DAL;
        public PostLogic()
        {
            try {
                Post_DAL = new PostDA();
                Image_DAL = new ImageDA();
                Helper_DAL = new HelperDA();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public bool AddPost(PostDTO post, ImageDTO image)
        {
            if (image != null)
            {
                try
                {
                    return (Image_DAL.Upload(image) && Post_DAL.AddPost(post));
                }
                catch(DataAccessException e)
                {
                    throw e;
                }
                catch(DataInsertException e)
                {
                    throw e;
                }
            }
            else
            {
                try {
                    return (Post_DAL.AddPost(post));
                }
                catch(DataAccessException e)
                {
                    throw e;
                }
            }

        }

        public List<PostDTO> GetNewPosts()
        {
            List<PostDTO> list = null;
            try
            {
                list = Post_DAL.GetPosts();
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            try {
                list.Sort((y, x) => DateTime.Compare(x.Date, y.Date));
            }
            catch(Exception e)
            {
                throw e;
            }
            return list;
        }

        public List<PostDTO> GetBestPosts()
        {
            List<PostDTO> list = null;
            try {
                list = Post_DAL.GetPosts();
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            try {
                list = list.OrderBy(o => o.Raiting).ThenBy(o => o.Date).ToList();
            }
            catch(Exception e)
            {
                throw e;
            }
            return list;
        }

        public List<PostDTO> GetHotPosts()
        {
            List<PostDTO> list = null;
            try {
                list = Post_DAL.GetPosts();
            }
            catch(DataAccessException e)
            {
                throw e;
            }
            try {
                list.Where(item => item.Date == DateTime.Today);
                list = list.OrderBy(o => o.Raiting).ThenBy(o => o.Date).ToList();
            }
            catch(Exception e)
            {
                throw e;
            }
            
            return list;
        }

        public PostDTO GetPostByID(int id)
        {
            try
            {
                return Post_DAL.GetPostByID(id);
            }
            catch(DataAccessException e)
            {
                throw e;
            }
        }

        public ImageDTO GetImage(Guid id)
        {
            try {
                return Image_DAL.GetImage(id);
            }
            catch(DataAccessException e)
            {
                throw e;
            }
        }

        public int AddComment(CommentDTO comment)
        {
            if (Post_DAL.AddComment(comment))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public List<CommentDTO> GetComments(int post_id)
        {
            return Post_DAL.GetComments(post_id);
        }

        public bool PostLike(int post_id, int user_id)
        {
            var author = Helper_DAL.GetUserByName(Post_DAL.GetPostByID(post_id).Author);
            if (!Post_DAL.IsUserPostLike(user_id, post_id))
            {
                Helper_DAL.Raiting(author.Id, 2);
                Post_DAL.AddUserPostLike(user_id, post_id);
                return Post_DAL.PostLike(post_id);
            }
            else
            {
                Helper_DAL.Raiting(author.Id, -2);
                Post_DAL.RemoveUserPostLike(user_id, post_id);
                return Post_DAL.PostDislike(post_id);
            }
        }




        public CommentDTO GetCommentByID(int id)
        {
            return Post_DAL.GetCommentByID(id);
        }

        public bool IsUserPostLike(int post_id, int user_id)
        {
            return Post_DAL.IsUserPostLike(user_id, post_id);
        }

        public bool IsUserCommentLike(int comment_id, int user_id)
        {
            return Post_DAL.IsUserCommentLike(user_id, comment_id);
        }

        public bool CommentLike(int comment_id, int user_id)
        {
            var author = Helper_DAL.GetUserByName(Post_DAL.GetCommentByID(comment_id).Author);
            if (!Post_DAL.IsUserCommentLike(user_id, comment_id))
            {
                Helper_DAL.Raiting(author.Id, 1);
                Post_DAL.AddUserCommentLike(user_id, comment_id);
                return Post_DAL.CommentLike(comment_id);
            }
            else
            {
                Helper_DAL.Raiting(author.Id, -1);
                Post_DAL.RemoveUserCommentLike(user_id, comment_id);
                return Post_DAL.CommentDislike(comment_id);
            }
        }

        public bool DeletePost(PostDTO post, UserDTO author)
        {
            if(author != null)
            {
                return Helper_DAL.Raiting(author.Id, post.Raiting * (-1)) && Post_DAL.DeletePost(post.Id);
            }
            else return Post_DAL.DeletePost(post.Id);
        }

        public bool DeleteComment(CommentDTO comment, UserDTO author)
        {
            return Post_DAL.DeleteComment(comment.Id);
        }
    }
}
