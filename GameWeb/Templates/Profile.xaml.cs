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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        public List<Login.User> Friends = new List<Login.User>();
        public List<GameEntity> Games = new List<GameEntity>();
        public Profile()
        {
            InitializeComponent();
            UserName.Text = MainWindow.user.Name;
            LastIn.Text = "Последний раз в сети: " + MainWindow.user.LastDate.ToString();
            DateFrom.Text = "Зарегестрирован: " + MainWindow.user.DateFrom.ToString();
            IsOnline.Foreground = Brushes.Green;
            try
            {
                UserAvatar.Source = new BitmapImage(new Uri(MainWindow.user.AvatarImg));
            }
            catch
            {
                UserAvatar.Source = new BitmapImage(new Uri("https://sun9-13.userapi.com/s/v1/ig2/04e4zWFRl5DMeRI_1n29tFA2PiWKi-3sjPfGUJ6w03UPz6P6P-5WWReZ_ujpWzsm8wQvWqHkuYMbPO6xsLwP1Fbw.jpg?size=1920x1920&quality=96&type=album"));
            }
            LoadFriends();
            LoadGames();
            IsReqFriends();
        }

        public void IsReqFriends()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT * FROM friends WHERE friendid={MainWindow.user.Id} AND confirm=0", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        IsReqFri.Source = new BitmapImage(new Uri("https://yt3.ggpht.com/a/AATXAJwFmlJNRkAYNSNOaXIhgIuDGs9CNZMyJYPYVQ=s900-c-k-c0xffffffff-no-rj-mo"));
                    }
                    connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось запросить уведомления о новых запросах в друзья, попробуйте позже");
            }
            
        }

        public void LoadGames()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT games.id,name,category,price,description,logoimgpath,backimgpath FROM games INNER JOIN sales ON games.id=sales.gameid WHERE sales.userid={MainWindow.user.Id}", connection);
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
                MessageBox.Show("Не удалось загрузить список игр, попробуйте позже");
            }
        }
        public void LoadFriends()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    int[] friends = new int[10];
                    int length = 0;
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"SELECT id,userid,friendid FROM friends WHERE userid={MainWindow.user.Id} AND confirm=1", connection);
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
                MessageBox.Show("Не удалось загрузить список друзей, попробуйте позже");
            }

           
        }

        private void ExitAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"UPDATE user SET online=0 WHERE id={MainWindow.user.Id}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MainWindow.MyForm.Visibility = Visibility.Hidden;
                Login.RegLog regLog = new Login.RegLog();
                regLog.Show();
            }
            catch
            {
                MainWindow.MyForm.Visibility = Visibility.Hidden;
                Login.RegLog regLog = new Login.RegLog();
                regLog.Show();
            }
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new Templates.EditProfile());
        }
        private void FindFriends_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new Templates.AllUsers());
        }

        private void FriendsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var us = FriendsList.SelectedItem as Login.User;
            Templates.AnyUser anyUser = new Templates.AnyUser(us);
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(anyUser);
        }

        private void Messages_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new Templates.Messages());
        }

        private void GamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = GamesList.SelectedItem as GameEntity;
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new CurrentGame(a));
        }

        private void SaveAcc_Click(object sender, RoutedEventArgs e)
        {
            LoadEmail loadEmail = new LoadEmail();
            loadEmail.Show();
        }
    }
}
