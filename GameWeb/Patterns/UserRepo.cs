using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Patterns
{
    public class UserRepo 
    {
        public void Create(int NewID,string NewLogin,string NewPassword, string NewName, bool Admin,string datefrom, string DefaultImage)
        {
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"INSERT INTO user (ID,login,password,name,admin,online,datefrom,avatarimg) VALUES ({NewID},'{NewLogin}','{NewPassword}','{NewName}',{Admin},0,'{datefrom}','{DefaultImage}')", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool CheckUser(string NewLogin, string NewName)
        {
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT * FROM user WHERE login = '{NewLogin}' OR name = '{NewName}'", connection);
                MySqlDataReader readerForCheckLogin = (MySqlDataReader)command.ExecuteReader();
                if (readerForCheckLogin.HasRows)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
            }
            return true;
        }

        public Login.User GetUser(string Ulogin,string Upassword)
        {
            Login.User user = null;
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT id,login,password,name,admin,online,datefrom,avatarimg FROM user WHERE login='{Ulogin}' AND password='{Upassword}'", connection);
                MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    while(reader.Read())
                    {
                        int id = int.Parse(reader.GetValue(0).ToString());
                        string login = reader.GetValue(1).ToString();
                        string password = reader.GetValue(2).ToString();
                        string name = reader.GetValue(3).ToString();
                        int admin = int.Parse(reader.GetValue(4).ToString());
                        string datefrom = reader.GetValue(6).ToString();
                        string lastdate = DateTime.Now.ToShortDateString();
                        string avatarimg = reader.GetValue(7).ToString();
                        user = new Login.User(id, login, password, name, admin, 1, datefrom, lastdate, avatarimg);
                        UpdateLastDate(lastdate, id);
                    }
                }
                connection.Close();
            }

            return user;
        }

        public void UpdateLastDate(string lastdate, int id)
        {
            using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"UPDATE user SET lastdate='{lastdate}',online=1 WHERE id={id}", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public int GetID()
        {
            try
            {
                int CurrentID = 0;
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {

                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM user ORDER BY id", connection);
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
                return CurrentID;
            }
            catch
            {
                return -1;
            }
            
        }

        public List<Login.User> LoadUsers()
        {
            List<Login.User> users = new List<Login.User>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,login,password,name,admin,online,datefrom,lastdate,avatarimg FROM user WHERE NOT ID={MainWindow.user.Id}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            string login = reader.GetValue(1).ToString();
                            string password = reader.GetValue(2).ToString();
                            string name = reader.GetValue(3).ToString();
                            int admin = int.Parse(reader.GetValue(4).ToString());
                            int online = int.Parse(reader.GetValue(5).ToString());
                            string datefrom = reader.GetValue(6).ToString();
                            string lastdate = reader.GetValue(7).ToString();
                            string avatarimg = reader.GetValue(8).ToString();

                            Login.User user = new Login.User(id, login, password, name, admin, online, datefrom, lastdate, avatarimg);
                            users.Add(user);
                        }
                    }

                    connection.Close();
                }
                return users;
            }
            catch
            {
                return null;
            }
        }
    }
}
