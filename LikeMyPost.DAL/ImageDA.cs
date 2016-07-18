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
    public class ImageDA : IImageDAL
    {
        public ImageDA()
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
        public ImageDTO GetImage(Guid id)
        {
            using (var connection = new SqlConnection(CS))
            {
                ImageDTO image = null;
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT format, data FROM dbo.Images WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    string format;
                    byte[] data;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            format = (string)reader["format"];
                            data = (byte[])reader["data"];
                            image = new ImageDTO(id, format, data);
                        }
                    }
                }
                catch
                {
                    throw new DataAccessException("Can't get Image");
                }

                return image;
            }
        }

        public bool Upload(ImageDTO image)
        {
            int summary = 0;
            using (var connection = new SqlConnection(CS))
            {
                try {
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO dbo.Images (id, format, data) VALUES(@GuidID, @Format, @Data) ";
                    command.Parameters.AddWithValue("@Format", image.Format);
                    command.Parameters.AddWithValue("@Data", image.Data);
                    command.Parameters.AddWithValue("@GuidID", image.Id);
                    connection.Open();
                    summary = command.ExecuteNonQuery();
                }
                catch
                {
                    throw new DataInsertException("Can't upload Image");
                }
            }
            return summary > 0;
        }
    }
}
