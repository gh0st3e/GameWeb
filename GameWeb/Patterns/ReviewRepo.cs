using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Patterns
{
    public class ReviewRepo 
    {
        public int GetID()
        {
            try
            {
                int CurrentID = 0;
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM reviews ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            CurrentID = id;
                        }
                    }
                    connection.Close();
                }
                return ++CurrentID;
            }
            catch
            {
                return -1;
            }
        }
        public void Create(int userid, string text)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    if(GetID()!=-1)
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO reviews (id,userid,review) VALUES ({GetID()},{userid},'{text}')", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
        }
        public List<Templates.ReviewEntity> LoadReviews()
        {
            try
            {
                List<Templates.ReviewEntity> reviews = new List<Templates.ReviewEntity>();
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT reviews.id,reviews.review,user.name,user.avatarimg FROM reviews INNER JOIN user on reviews.userid=user.id ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string review = reader.GetValue(1).ToString();
                            string name = reader.GetValue(2).ToString();
                            string avatarimage = reader.GetValue(3).ToString();
                            Templates.ReviewEntity newreview = new Templates.ReviewEntity(id, avatarimage, name, review);
                            reviews.Add(newreview);
                        }
                    }
                    connection.Close();
                }
                return reviews;
            }
            catch
            {
                return null;
            }
        }
    }
}
