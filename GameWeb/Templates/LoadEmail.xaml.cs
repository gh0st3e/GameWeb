using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameWeb.Templates
{
    /// <summary>
    /// Логика взаимодействия для LoadEmail.xaml
    /// </summary>
    public partial class LoadEmail : Window
    {
        public string s;
        public string mail;
        public int CurrentID;
        public LoadEmail()
        {
            InitializeComponent();
            Random r = new Random();
            var x = r.Next(0, 1000000);
            s = x.ToString("000000");
            
        }

        private void SendMail_Click(object sender, RoutedEventArgs e)
        {
            mail = UserEmail.Text;
            try
            {
                MailAddress from = new MailAddress("for.kyrs@gmail.com", "GameWeb");
                MailAddress to = new MailAddress(UserEmail.Text);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Код подтверждения";
                m.Body = "Ваш код подтверждения: " + s;
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("for.kyrs@gmail.com", "Kyrs1234");
                smtp.EnableSsl = true;
                smtp.Send(m);
                MessageBox.Show("Письмо отправлено");
                Text.Visibility = Visibility.Visible;
                Confirm.Visibility = Visibility.Visible;
                ConfirmCode.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Письмо не отправлено, проверьте правильность введенных данных или подключение к интернету");
            }
        }

        
        public void GetID()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM secondauth ORDER BY id", connection);
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

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if(ConfirmCode.Text == s)
            {
                try
                {
                    GetID();
                    using(MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"INSERT INTO secondauth(id,userid,email) VALUES ({++CurrentID},{MainWindow.user.Id},'{mail}')", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Ваш аккаунт защищен");
                    }
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка, попробуйте позже");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Неверный код");
                Text.Visibility = Visibility.Hidden;
                Confirm.Visibility = Visibility.Hidden;
                ConfirmCode.Visibility = Visibility.Hidden;
                Random r = new Random();
                var x = r.Next(0, 1000000);
                s = x.ToString("000000");
            }
        }
    }
}
