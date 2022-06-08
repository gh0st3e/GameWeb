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
    /// Логика взаимодействия для CreateNew.xaml
    /// </summary>
    public partial class CreateNew : UserControl
    {
        public bool isnewsimage = false;
        public int CurrentID = 0;
        public CreateNew()
        {
            InitializeComponent();
            isnewsimage = false;
        }

        public void GetID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM news ORDER BY id", connection);
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

        private void CreateNewsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NewsName.Text.Length < 10)
            {
                MessageBox.Show("Закголовое новости слишком короткий");
                return;
            }
            if (NewsName.Text.Length > 50)
            {
                MessageBox.Show("Закголовое новости слишком длинный");
                return;
            }
            if (NewsText.Text.Length<30)
            {
                MessageBox.Show("Текст новости слишком короткий");
                return;
            }
            if (NewsText.Text.Length > 1000)
            {
                MessageBox.Show("Текст новости слишком длинный");
                return;
            }
            if (!isnewsimage)
            {
                MessageBox.Show("Некорректное изображение");
                return;
            }
            else
            {
                using(MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT * FROM news WHERE name='{NewsName.Text}'", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Новость с таким название уже существует");
                            connection.Close();
                            return;
                        }
                        else
                        {
                            try
                            {
                                GetID();
                                connection.Close();
                                connection.Open();
                                command = new MySqlCommand($"INSERT INTO news(id,name,image,descr) VALUES ({++CurrentID},'{NewsName.Text}','{NewsImage.Text}','{NewsText.Text}')", connection);
                                command.ExecuteNonQuery();
                                connection.Close();
                                MessageBox.Show("Новость добавлена");
                            }
                            catch
                            {
                                MessageBox.Show("Произошла ошибка при добавлении новости, проверьте правильность введеных данных или проверьте подключение к интернету");
                                return;
                            }

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Проверьте подключение к интернету");
                        return;
                    }
                   
                }
            }
        }

        private void NewsImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ImageEx.Source = new BitmapImage(new Uri(NewsImage.Text));
                isnewsimage = true;
            }
            catch
            {
                isnewsimage = false;
            }
            
        }
    }
}
