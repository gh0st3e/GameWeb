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
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News : UserControl
    {
        public int[] rn = new int[3];
        public List<NewsEntity> news = new List<NewsEntity>();
        public News()
        {
            InitializeComponent();
            LoadNews();
            if(MainWindow.user.Admin==1)
            {
                CreateNews.Visibility = Visibility.Visible;
            }
            else
            {
                CreateNews.Visibility = Visibility.Hidden;
            }
        }
        public void LoadNews()
        {
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    int k = 0;
                  
                
                        //WHERE id={rn[k]}"
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"SELECT id,name,image,descr FROM news ORDER BY RAND() LIMIT 3", connection);
                        MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read()) // построчно считываем данные
                            {
                                int id = int.Parse(reader.GetValue(0).ToString());
                                string name = reader.GetValue(1).ToString();
                                string image = reader.GetValue(2).ToString();
                                string descr = reader.GetValue(3).ToString();
                                NewsEntity newsentity = new NewsEntity(id, name, image, descr);                  
                                news.Add(newsentity);
                            }
                        }
                        connection.Close();
                }
                MainImage1.Source = new BitmapImage(new Uri(news[0].Image));
                MainName1.Text = news[0].Name;
                MainImage2.Source = new BitmapImage(new Uri(news[1].Image));
                MainName2.Text = news[1].Name;
                MainImage3.Source = new BitmapImage(new Uri(news[2].Image));
                MainName3.Text = news[2].Name;
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить список новостей, попробуйте позже или проверьте сове интернет-соединение");
            }

            
        }
        

        private void SeeMore1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new CurrentNew(news[0]));
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить новость, попробуйте позже или проверьте соединение с интернетом");
            }
            
        }

        private void SeeMore2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new CurrentNew(news[1]));
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить новость, попробуйте позже или проверьте соединение с интернетом");
            }
            
        }

        private void SeeMore3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new CurrentNew(news[2]));
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить новость, попробуйте позже или проверьте соединение с интернетом");
            }
            
        }

        private void CreateNews_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new CreateNew());
        }
    }
}
