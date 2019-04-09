using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ImageSharePassword.Data;

namespace ImageSharePassword.Data
{
    public class ImageDb
    {
        private string _connectionString;

        public ImageDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Image image, int UserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Images " +
                                  "VALUES (@fileName, @password, 0, @UserId) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@fileName", image.FileName);
                cmd.Parameters.AddWithValue("@password", image.Password);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                image.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        

        public Image GetById(int id)
        {
            
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT TOP 1 * FROM Images WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }

                return new Image
                {
                    Id = (int)reader["Id"],
                    Password = (string)reader["Password"],
                    FileName = (string)reader["FileName"],
                    Views = (int)reader["Views"]
                };
            }
        }

        public IEnumerable<Image> AllImagesForUser(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM Images WHERE userId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            var reader = cmd.ExecuteReader();
            List<Image> images = new List<Image>();


            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    Password = (string)reader["Password"],
                    FileName = (string)reader["FileName"],
                    Views = (int)reader["Views"]
                });
            }
            return images;
        }

        public void IncrementViewCount(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE Images SET Views = Views + 1 WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}