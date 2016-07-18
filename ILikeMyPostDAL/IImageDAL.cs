using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeMyPost.Entities;

namespace ILikeMyPostDAL
{
    public interface IImageDAL
    {
        bool Upload(ImageDTO image);
        ImageDTO GetImage(Guid id);
    }
}
