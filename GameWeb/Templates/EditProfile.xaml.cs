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
    /// Логика взаимодействия для EditProfile.xaml
    /// </summary>
    public partial class EditProfile : UserControl
    {
        public bool NameGhanged = false;
        public bool ImageChanged = false;
        
        public EditProfile()
        {
            InitializeComponent();
            NameGhanged = false;
            ImageChanged = false;
            urlforimage.Text = MainWindow.user.AvatarImg;
            newusername.Text = MainWindow.user.Name;
            try
            {
                avatarimage.Source = new BitmapImage(new Uri(MainWindow.user.AvatarImg));
            }
            catch
            {
                
            }
        }

        private void urlforimage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                avatarimage.Source = new BitmapImage(new Uri(urlforimage.Text));
                ImageChanged = true;
            }
            catch
            {

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new Templates.Profile());
        }

        private void DelAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM user WHERE id={MainWindow.user.Id}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MainWindow.MyForm.Visibility = Visibility.Hidden;
                Login.RegLog regLog = new Login.RegLog();
                regLog.Show();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка, попорбуйте позже");
            }
        }

        private void ChangeConfirm_Click(object sender, RoutedEventArgs e)
        {
            
            string NewName;
            string NewPassword;
            string NewImage;
            if(ImageChanged)
            {
                if(urlforimage.Text!="")
                {
                    NewImage = urlforimage.Text;
                }
                else
                {
                    MessageBox.Show("Ссылка на изображение не может быть пуста");
                    return;
                }
                
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"UPDATE user SET avatarimg='{NewImage}' WHERE id={MainWindow.user.Id}", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MainWindow.user.AvatarImg = NewImage;
                    }
                }
                catch
                {
                    MessageBox.Show("Не удалось обновить изображение, попробуйте позже");
                }
            }
            

            if(NameGhanged && newusername.Text != MainWindow.user.Name)
            {
                if(newusername.Text.Length>3)
                {
                    NewName = newusername.Text;
                }
                else
                {
                    MessageBox.Show("Недостаточная длина имени");
                    return;
                }
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"UPDATE user SET name='{NewName}' WHERE id={MainWindow.user.Id}", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MainWindow.user.Name = NewName;
                    }
                }
                catch
                {
                    MessageBox.Show("Не удалось обновить имя пользователя, попробуйте позже");
                }
            }
            

            if (oldpassword.Password != null && oldpassword.Password == MainWindow.user.Password && newpassword.Password == confirmpassword.Password)
            {
                NewPassword = newpassword.Password;
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"UPDATE user SET password='{NewPassword}' WHERE id={MainWindow.user.Id}", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MainWindow.user.Password = NewPassword;
                    }
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить пароль, попробуйте позже");
                }
               
            }
            else if (oldpassword.Password == "" && newpassword.Password == "" && confirmpassword.Password == "")
            {

            }
            else
            {
                MessageBox.Show("Вы допустили ошибку при измении пароля, попробуйте еще раз");
                return;
            }
            MainWindow.MyForm.FieldForTemplate.Children.Clear();
            MainWindow.MyForm.FieldForTemplate.Children.Add(new Profile());
            
        }

        private void newusername_TextChanged(object sender, TextChangedEventArgs e)
        {
            NameGhanged = true;
        }
    }
}
