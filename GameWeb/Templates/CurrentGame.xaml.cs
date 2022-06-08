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
using System.Diagnostics;

namespace GameWeb.Templates
{
    /// <summary>
    /// Логика взаимодействия для CurrentGame.xaml
    /// </summary>
    public partial class CurrentGame : UserControl
    {
        public GameEntity Game;
        public CurrentGame(GameEntity game)
        {
            InitializeComponent();
            Game = game;
            FullGameImage.Source = new BitmapImage(new Uri(Game.BackImgPath));
         
            if(Game.Name=="Snake Together")
            {
                PlayBtn.Visibility = Visibility.Visible;
            }
        }

        private void DelGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM sales WHERE userid={MainWindow.user.Id} AND gameid={Game.ID}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Игра удалена с акканута");
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Profile());
            }
            catch
            {
                MessageBox.Show("Не удалось удалить игру с аккаунта, попробуйте позже");
            }
           
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "SnakeGIT.exe";
            p.Start();
        }
    }
}
