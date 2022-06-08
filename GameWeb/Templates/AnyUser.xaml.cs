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
    /// Логика взаимодействия для AnyUser.xaml
    /// </summary>
    public partial class AnyUser : UserControl
    {
        public int ThisUserId;
        public Login.User frienduser;

        public List<Login.User> Friends = new List<Login.User>();
        public List<GameEntity> Games = new List<GameEntity>();
        public AnyUser(Login.User user)
        {
            InitializeComponent();

            frienduser = user;
            ThisUserId = user.Id;
            LastIn.Text = "Последний раз в сети: " + user.LastDate.ToString();
            DateFrom.Text = "Зарегестрирован: " + user.DateFrom.ToString();
            UserName.Text = user.Name;
            if(user.Online==1)
            {
                IsOnline.Foreground = Brushes.Green;
            }
            
            try
            {
                UserAvatar.Source = new BitmapImage(new Uri(user.AvatarImg));
            }
            catch
            {
                UserAvatar.Source = new BitmapImage(new Uri("https://sun9-13.userapi.com/s/v1/ig2/04e4zWFRl5DMeRI_1n29tFA2PiWKi-3sjPfGUJ6w03UPz6P6P-5WWReZ_ujpWzsm8wQvWqHkuYMbPO6xsLwP1Fbw.jpg?size=1920x1920&quality=96&type=album"));
            }
            CheckFriendShip();
            LoadFriends();
            LoadGames();
        }

        public void LoadGames()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT games.id,name,category,price,description,logoimgpath,backimgpath FROM games INNER JOIN sales ON games.id=sales.gameid WHERE sales.userid={frienduser.Id}", connection);
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
                            Games.Add(game);
                        }
                    }
                    connection.Close();
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = Games;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка загрузки игр пользователя, попробуйте позже или проверьте соединение с интернетом");
            }
            
        }

        public void LoadFriends()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    int[] friends = new int[10];
                    int length = 0;
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,userid,friendid FROM friends WHERE userid={frienduser.Id} AND confirm=1", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            friends[length] = int.Parse(reader.GetValue(2).ToString());
                            length++;

                        }
                    }
                    connection.Close();
                    int count = 0;
                    for (; length > 0; length--, count++)
                    {
                        connection.Open();
                        command = new MySqlCommand($"SELECT id,login,password,name,admin,online,datefrom,lastdate,avatarimg FROM user WHERE id = {friends[count]} ", connection);
                        reader = (MySqlDataReader)command.ExecuteReader();
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
                                Friends.Add(user);

                            }
                        }
                        connection.Close();
                    }
                    FriendsList.ItemsSource = null;
                    FriendsList.ItemsSource = Friends;

                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка загрузки друзей пользователя, попробуйте позже или проверьте соединение с интернетом");
            }

            
        }

        public void CheckFriendShip()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT * FROM friends WHERE userid={MainWindow.user.Id} AND friendid={ThisUserId}", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        AddFriend.Visibility = Visibility.Hidden;
                        DelFriend.Visibility = Visibility.Visible;
                    }
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("Произошла ошибка проверки дружбы или у вас отсутствует интернет-соединение");
            }
        }
        public int GetID()
        {
            int currentID=0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM friends ORDER BY id", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            currentID = id;
                        }
                    }
                    connection.Close();
                }
                
                return currentID;
            }
            catch
            {
                return 0;
            }
            
        }

        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO friends VALUES({GetID()+1},{MainWindow.user.Id},{ThisUserId},0)", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Запрос отправлен");
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Profile());
            }
            catch
            {
                MessageBox.Show("Проверьте ваше интернет соединение");
            }
        }

        private void DelFriend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM friends WHERE userid={MainWindow.user.Id} AND friendid={ThisUserId}", connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM friends WHERE userid={ThisUserId} AND friendid={MainWindow.user.Id}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Друг удален");
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Profile());
            }
            catch
            {
                MessageBox.Show("Ошибка удаления пользователя из друзей, попробуйте позже или проверьте ваше интернет соединение");
            }
        }

        private void OpenChat_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new ChatControl(frienduser));
        }
    }
}
