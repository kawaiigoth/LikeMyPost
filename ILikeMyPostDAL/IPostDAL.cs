using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;

namespace ILikeMyPostDAL
{
    public interface IPostDAL
    {
        bool DeleteComment(int comment_id);
        bool DeletePost(int post_id);
        CommentDTO GetCommentByID(int id);
        bool PostLike(int post_id);
        bool PostDislike(int post_id);
        bool IsUserPostLike(int user_id, int post_id);
        void RemoveUserPostLike(int user_id, int post_id);
        void AddUserPostLike(int user_id, int post_id);
        bool CommentLike(int comment_id);
        bool CommentDislike(int comment_id);
        bool IsUserCommentLike(int user_id, int comment_id);
        void RemoveUserCommentLike(int user_id, int comment_id);
        void AddUserCommentLike(int user_id, int comment_id);
        bool AddComment(CommentDTO comment);
        bool AddPost(PostDTO post);
        List<PostDTO> GetPosts();
        List<CommentDTO> GetComments(int post_id);
        PostDTO GetPostByID(int id);
    }
}
