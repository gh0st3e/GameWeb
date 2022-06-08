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

namespace GameWeb
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        
        public static string connectionString = "server = MYSQL8001.site4now.net;" +

                "username=a86ac7_gameweb;" +
                "password=Denis2003;" +
                "database=db_a86ac7_gameweb";
        public static Login.User user;
        public static MainWindow MyForm;
        
        public MainWindow()
        {
            InitializeComponent();
            MyForm = this;
            this.MaxHeight = 900;
            this.MaxWidth = 1600;
            this.MinHeight = 900;
            this.MinWidth = 1600;
            this.Visibility = Visibility.Hidden;
            Login.RegLog regLog = new Login.RegLog();
            regLog.Show();
            MainNews.Foreground = Brushes.Blue;
           

        }

        public void OnUserChange()
        {
            if (user != null)
            {
                FieldForTemplate.Children.Clear();
                FieldForTemplate.Children.Add(new Templates.News());
                MainProfile.Content = user.Name;
                
            }
        }

        

        private void MainNews_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Templates.News());
            MainNews.Foreground = Brushes.Blue;
            MainShop.Foreground = Brushes.Black;
            MainGames.Foreground = Brushes.Black;
            MainProfile.Foreground = Brushes.Black;
            
        }

        private void MainShop_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Templates.Shop());
            MainNews.Foreground = Brushes.Black;
            MainShop.Foreground = Brushes.Blue;
            MainGames.Foreground = Brushes.Black;
            MainProfile.Foreground = Brushes.Black;
        }

        private void MainGames_Click(object sender, RoutedEventArgs e)
        {
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(new Templates.Reviews());
            MainNews.Foreground = Brushes.Black;
            MainShop.Foreground = Brushes.Black;
            MainGames.Foreground = Brushes.Blue;
            MainProfile.Foreground = Brushes.Black;
        }

        private void MainProfile_Click(object sender, RoutedEventArgs e)
        {
            Templates.Profile profile = new Templates.Profile();
            FieldForTemplate.Children.Clear();
            FieldForTemplate.Children.Add(profile);
            MainNews.Foreground = Brushes.Black;
            MainShop.Foreground = Brushes.Black;
            MainGames.Foreground = Brushes.Black;
            MainProfile.Foreground = Brushes.Blue;

        }

        
    }
}
