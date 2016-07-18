using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPostBLL
{
    public interface IPostBLL
    {
        bool DeleteComment(CommentDTO comment, UserDTO author);
        bool DeletePost(PostDTO post, UserDTO author);
        bool IsUserPostLike(int post_id, int user_id);
        bool IsUserCommentLike(int comment_id, int user_id);
        bool PostLike(int post_id, int user_id);
        bool CommentLike(int comment_id, int user_id);
        int AddComment(CommentDTO comment);
        bool AddPost(PostDTO post, ImageDTO image);
        List<PostDTO> GetNewPosts();
        List<PostDTO> GetHotPosts();
        List<PostDTO> GetBestPosts();
        List<CommentDTO> GetComments(int post_id);
        PostDTO GetPostByID(int id);
        CommentDTO GetCommentByID(int id);
        ImageDTO GetImage(Guid id);
    }
}
