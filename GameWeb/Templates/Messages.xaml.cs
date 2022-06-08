using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameWeb.Templates
{
    /// <summary>
    /// Логика взаимодействия для Messages.xaml
    /// </summary>
    ///

   
    public partial class Messages : UserControl
    {
        public List<Login.User> users = new List<Login.User>();
        public int CurrentID;
        public Messages()
        {
            InitializeComponent();
            GetMessages();
            MessagesList.ItemsSource = null;
            MessagesList.ItemsSource = users;
        }

        public int GetId()
        {
            using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT id FROM friends ORDER BY id", connection);
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
        public void GetMessages()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT userid,friendid FROM friends WHERE confirm=0 AND friendid={MainWindow.user.Id}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            int friendid = int.Parse(reader.GetValue(1).ToString());
                            ShowFriend(id);


                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить уведомления, попробуйте позже");
            }
            
        }
        public void ShowFriend(int YourId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,login,password,name,admin,online,datefrom,lastdate,avatarimg FROM user WHERE id = {YourId}", connection);
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
            }
            catch
            {
                MessageBox.Show("Проверьте интернет-соединение");
            }
            
        }

        private void AcceptFriend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = int.Parse(e.Source.ToString().Last().ToString());
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"UPDATE friends SET confirm=1 WHERE userid={index} AND friendid={MainWindow.user.Id}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    command = new MySqlCommand($"INSERT INTO friends VALUES({GetId() + 1},{MainWindow.user.Id},{index},1)", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessagesList.ItemsSource = null;
                    users.Clear();
                    GetMessages();
                    MessagesList.ItemsSource = users;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось принять запрос, попробуйте позже");
            }
                
        }

        private void CancelFriend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = int.Parse(e.Source.ToString().Last().ToString());
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM friends WHERE userid={index} AND friendid={MainWindow.user.Id}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessagesList.ItemsSource = null;
                    users.Clear();
                    GetMessages();
                    MessagesList.ItemsSource = users;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось отменить запрос, попробуйте позже");
            }
            
        }
    }
}
