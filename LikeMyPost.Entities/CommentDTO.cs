using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class CommentDTO
    {
        public int Id { get; private set; }
        public string Author { get; private set; }
        public DateTime Date { get; private set; }
        public string Text { get; private set; }
        public int Post_Id { get; private set; }
        public int Raiting { get; private set; }
        public List<string> Voters { get; private set; }
        public CommentDTO(string author, string text, int post_id) 
        {
            Author = author;
            Post_Id = post_id;
            Text = text;
            Date = DateTime.Now;
        }
        public CommentDTO(int id, string author, string text, int post_id, DateTime date, int raiting)
        {
            Id = id;
            Author = author;
            Post_Id = post_id;
            Text = text;
            Date = date;
            Raiting = raiting;
        }
    }
}
