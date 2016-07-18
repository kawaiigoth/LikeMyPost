using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeMyPost.Entities
{
    public class ImageDTO
    {
        public Guid Id { get; private set; }
        public string Format { get; private set; }
        public byte[] Data { get; private set; }

        public ImageDTO(string format, byte[] data)
        {
            Id = Guid.NewGuid();
            Format = format;
            Data = data;
        }

        public ImageDTO(Guid id, string format, byte[] data) // Load from DB
        {
            Id = id;
            Format = format;
            Data = data;
        }
    }
}
