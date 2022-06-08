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
    /// Логика взаимодействия для NewGame.xaml
    /// </summary>
    public partial class NewGame : UserControl
    {
        public bool islogoimage = false;
        public bool isfullimage = false;
        public int CurrentID = 0;
        public NewGame()
        {
            InitializeComponent();
            GameCategory.Items.Add("RPG");
            GameCategory.Items.Add("Шутеры");
            GameCategory.Items.Add("MOBA");
            GameCategory.Items.Add("Стратегии");
            GameCategory.Items.Add("GameWeb");
            GameCategory.SelectedIndex = 0;
        }

        public void GetID()
        {
            try
            {
                
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM games ORDER BY id", connection);
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
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к интернету, попробуйте позже");
                return;
            }
            
        }

        private void CreateGameBtn_Click(object sender, RoutedEventArgs e)
        {
            if(GameName.Text.Length<3)
            {
                MessageBox.Show("Слишком короткое название игры");
                return;
            }
            if (GameName.Text.Length > 50)
            {
                MessageBox.Show("Слишком длинное название игры");
                return;
            }
            if (GameDescr.Text.Length<20)
            {
                MessageBox.Show("Слишком короткое описание игры");
                return;
            }
            if (GameDescr.Text.Length > 200)
            {
                MessageBox.Show("Слишком длинное описание игры");
                return;
            }
            if (!islogoimage || !isfullimage)
            {
                MessageBox.Show("Некорректные изображения, попробуйте другие");
            }
            else
            {
                try
                {
                    using ( MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        GetID();
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO games(id,name,category,price,description,logoimgpath,backimgpath) VALUES ({++CurrentID},'{GameName.Text}','{GameCategory.SelectedItem.ToString()}',0,'{GameDescr.Text}','{GameLogo.Text}','{GameImage.Text}')", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    MessageBox.Show("Игра успешно добавлена");
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения к интернету, попробуйте позже");
                    return;
                }
            }
        }

        private void GameLogo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                LogoImg.Source = new BitmapImage(new Uri(GameLogo.Text));
                
                    islogoimage = true;
                
                
            }
            catch
            {
                islogoimage = false;
            }
        }

        private void GameImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FullImg.Source = new BitmapImage(new Uri(GameImage.Text));
                isfullimage = true;
            }
            catch
            {
                isfullimage = false;
            }
        }
    }
}
