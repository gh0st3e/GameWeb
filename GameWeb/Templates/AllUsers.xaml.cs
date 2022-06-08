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
    /// Логика взаимодействия для AllUsers.xaml
    /// </summary>
    public partial class AllUsers : UserControl
    {
        public List<Login.User> users = new List<Login.User>();
        public Patterns.UnitOfWork UnitOfWork = new Patterns.UnitOfWork();
        public AllUsers()
        {
            InitializeComponent();
            if (UnitOfWork.UserRepo.LoadUsers() != null)
            {
                users = UnitOfWork.UserRepo.LoadUsers();
                UsersList.ItemsSource = null;
                UsersList.ItemsSource = users;
            }
            
        }
        

        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Templates.AnyUser anyUser = new Templates.AnyUser(users[UsersList.SelectedIndex]);
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(anyUser);
        }

       

        private void FindUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            List<Login.User> FoundUsers = new List<Login.User>();
            Regex regex = new Regex(FindUser.Text);
            foreach (Login.User user in users)
            {
                if (regex.IsMatch(user.Name))
                {
                    FoundUsers.Add(user);
                }
            }
            UsersList.ItemsSource = null;
            UsersList.ItemsSource = FoundUsers;
        }
    }
}
