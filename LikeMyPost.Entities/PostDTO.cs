using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class PostDTO
    {
        public int Id { get; private set; }
        public string Author { get; private set; }
        public int Raiting { get; private set; }
        public DateTime Date { get; private set; }
        public string Heading { get; private set; }
        public string Text { get; private set; }
        public Guid Image { get; private set; }
        public List<string> Voters { get; private set; }
        //public int NumberOfComments { get; private set; }


        public PostDTO(string heading, Guid img, string text, string author) // Create Post with Image
        {
            Author = author;
            Heading = heading;
            Image = img;
            Text = text;
            Date = DateTime.Now;
        }

        public PostDTO(string heading, string text, string author)  //Create Post without Image
        {
            Author = author;
            Heading = heading;
            Image = Guid.Empty;
            Text = text;
            Date = DateTime.Now;
        }

        public PostDTO(int id, string author, DateTime date, string heading, string text, Guid image, int raiting) // Loaded from DB
        {
            Id = id;
            Author = author;
            Heading = heading;
            Image = image;
            Text = text;
            Date = date;
            Raiting = raiting;
        }

        //public PostDTO(int user_id, string heading, string text, ImageDTO image) // Create Post
        //{
        //    Author_Id = user_id;
        //    Raiting = 0;
        //    Date = DateTime.Now;
        //    Heading = heading;
        //    Text = text;
        //    Image = image;
        //}

        //public PostDTO(int user_id, int raiting, DateTime date, string heading, string text) // post model without Id and Image, ready to add to DB
        //{
        //    Author_Id = user_id;
        //    Raiting = raiting;
        //    Date = date;
        //    Heading = heading;
        //    Text = text;
        //    Image_Id = Guid.Empty;
        //}

        //public PostDTO(int id, int user_id, int raiting, DateTime date, string heading, string text, Guid image_id, int comments) // post model with Id, received from DB
        //{
        //    Id = id;
        //    Author_Id = user_id;
        //    Raiting = raiting;
        //    Date = date;
        //    Heading = heading;
        //    Text = text;
        //    Image_Id = image_id;
        //    NumberOfComments = comments;
        //}
    }
}
