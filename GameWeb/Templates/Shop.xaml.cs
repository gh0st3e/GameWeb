using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Shop.xaml
    /// </summary>
    public partial class Shop : UserControl
    {
        public List<GameEntity> games = new List<GameEntity>();
        public List<GameEntity> FoundGames = new List<GameEntity>();
        public int choosedId;
       

        public Patterns.UnitOfWork UnitOfWork = new Patterns.UnitOfWork();
        public Shop()
        {
            InitializeComponent();
            
            Categories.Items.Add("Все");
            Categories.Items.Add("RPG");
            Categories.Items.Add("Шутеры");
            Categories.Items.Add("MOBA");
            Categories.Items.Add("Стратегии");
            Categories.Items.Add("GameWeb");
            Categories.SelectedIndex = 0;

            if(UnitOfWork.GameRepo.GetGames() != null)
            {
                games = UnitOfWork.GameRepo.GetGames();
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = games;
                GamesList.SelectedIndex = 0; 
            }
            else
            {
                MessageBox.Show("Ошибка получения списка игр, попробуйте позже или обратитесь в тех. поддержку");
            }
            if(MainWindow.user.Admin==1)
            {
                AddNewGame.Visibility = Visibility.Visible;
                DelGame.Visibility = Visibility.Visible;
            }
            else
            {
                AddNewGame.Visibility = Visibility.Hidden;
                DelGame.Visibility = Visibility.Hidden;
            }
        }
        
        

        private void GamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (GamesList.SelectedItem != null)
            {
                
                var a = GamesList.SelectedItem as GameEntity;
                choosedId = a.ID;
                if (UnitOfWork.GameRepo.CheckBought(choosedId))
                {
                    ByuGameBtn.Visibility = Visibility.Hidden;
                }
                else ByuGameBtn.Visibility = Visibility.Visible;
                BackImage.Source = new BitmapImage(new Uri(a.BackImgPath));
                GameName.Text = a.Name;
                GameDescr.Text = a.Descr;
            }
        }

        private void ByuGameBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UnitOfWork.GameRepo.BuyGame(choosedId);
                MessageBox.Show("Игра добавлена на аккаунт");
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Shop());
            }
            catch
            {
                MessageBox.Show("Не удалось добавить игру на акканут, попробуйте позже");
            }
            
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Categories.SelectedIndex==0)
            {
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = games;
            }
            if(Categories.SelectedIndex==1)
            {
                FoundGames.Clear();
                var Found = from p in games
                                 where p.Category == "RPG"
                                 select p;
                foreach(GameEntity game in Found)
                {
                    FoundGames.Add(game);
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = FoundGames;
            }
            if (Categories.SelectedIndex == 2)
            {
                FoundGames.Clear();
                var Found = from p in games
                            where p.Category == "Шутеры"
                            select p;
                foreach (GameEntity game in Found)
                {
                    FoundGames.Add(game);
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = FoundGames;
            }
            if (Categories.SelectedIndex == 3)
            {
                FoundGames.Clear();
                var Found = from p in games
                            where p.Category == "MOBA"
                            select p;
                foreach (GameEntity game in Found)
                {
                    FoundGames.Add(game);
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = FoundGames;
            }
            if (Categories.SelectedIndex == 4)
            {
                FoundGames.Clear();
                var Found = from p in games
                            where p.Category == "Стратегии"
                            select p;
                foreach (GameEntity game in Found)
                {
                    FoundGames.Add(game);
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = FoundGames;
            }
            if (Categories.SelectedIndex == 5)
            {
                FoundGames.Clear();
                var Found = from p in games
                            where p.Category == "GameWeb"
                            select p;
                foreach (GameEntity game in Found)
                {
                    FoundGames.Add(game);
                }
                GamesList.ItemsSource = null;
                GamesList.ItemsSource = FoundGames;
            }
        }

        private void FindStr_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<GameEntity> foundstrgames = new List<GameEntity>();
            Regex regex = new Regex(FindStr.Text);
            if(Categories.SelectedIndex==0)
            {
                foreach(GameEntity game in games)
                {
                    if(regex.IsMatch(game.Name))
                    {
                        foundstrgames.Add(game);
                    }
                }
            }
            else
            {
                foreach(GameEntity game in FoundGames)
                {
                    if(regex.IsMatch(game.Name))
                    {
                        foundstrgames.Add(game);
                    }
                }
            }
            GamesList.ItemsSource = null;
            GamesList.ItemsSource = foundstrgames;
        }

        

        private void AddNewGame_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new NewGame());
        }

        private void DelGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(MySqlConnection conection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    conection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM sales WHERE gameid={choosedId}", conection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"DELETE FROM games WHERE ID={choosedId}", conection);
                    command.ExecuteNonQuery();
                    conection.Close();
                    MessageBox.Show("Игра удалена");
                    MainWindow.MyForm.FieldForTemplate.Children.Clear();
                    MainWindow.MyForm.FieldForTemplate.Children.Add(new Shop());
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить игру из каталога, попробуйте позже или проверьте соединение с интернетом");
            }
        }
    }
}
