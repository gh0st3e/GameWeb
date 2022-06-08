using GameWeb.Templates;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Patterns
{
    public class GameRepo
    {
        public int GetID()
        {
            int CurrentID = 0;
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT id FROM sales ORDER BY id", connection);
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
        public List<GameEntity> GetGames()
        {
            try
            {
                List<GameEntity> games = new List<GameEntity>();
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id,name,category,price,description,logoimgpath,backimgpath FROM games", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string name = reader.GetValue(1).ToString();
                            string category = reader.GetValue(2).ToString();
                            int price = int.Parse(reader.GetValue(3).ToString());
                            string decription = reader.GetValue(4).ToString();
                            string logoimgpath = reader.GetValue(5).ToString();
                            string backimgpath = reader.GetValue(6).ToString();
                            GameEntity game = new GameEntity(id, name, category, price, decription, logoimgpath, backimgpath);
                            games.Add(game);
                        }
                    }
                    connection.Close();
                }
                return games;
            }
            catch
            {
                return null;
            }
        }
        public void BuyGame(int choosedId)
        {
            
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"INSERT INTO sales (id,userid,gameid) VALUES ({GetID()},{MainWindow.user.Id},{choosedId})", connection);
                command.ExecuteNonQuery();
                connection.Close();
               
            }
        }
        public bool CheckBought(int choosedId)
        {
            using(MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT * FROM sales WHERE userid={MainWindow.user.Id} AND gameid={choosedId}", connection);
                MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else return false;
            }
        }
    }
}
