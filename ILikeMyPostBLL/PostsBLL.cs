using LikeMyPost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeMyPost.BLL
{
    public interface PostsBLL
    {
        List<PostDTO> GetNewPosts();
        List<PostDTO> GetHotPosts();
        List<PostDTO> GetBestPosts();
        List<PostDTO> GetPostsByAuthor(int author_id);
        bool AddPost(PostDTO post);
        bool BanPost(int post_id);
        bool UnbanPost(int post_id);
        bool EditPost(PostDTO post);
        PostDTO GetPostById(int post_id);
        bool AddComment(CommentDTO comment);
        bool EditComment(CommentDTO comment);
        bool UpVotePost(int post_id, int voter_id);
        bool DownVotePost(int post_id, int voter_id);
        bool UpVoteComment(int comment_id, int voter_id);
        bool DownVoteComment(int comment_id, int voter_id);
        bool AddImage(ImageDTO image);
        ImageDTO GetImage(Guid image_id);
    }
}
