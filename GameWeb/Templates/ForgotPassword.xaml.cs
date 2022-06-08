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
    /// Логика взаимодействия для ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public string s;
        public string mail;
        public int CurrentID;
        public ForgotPassword()
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
                m.Subject = "Код подтверждения для восстановления аккаунта";
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

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmCode.Text == s)
            {
                try
                {
                    
                    using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"UPDATE user INNER JOIN secondauth ON user.id=secondauth.userid SET password=1234 WHERE secondauth.email='{mail}' ", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Пароль аккаунта сменен на 1234, не забудьте поменять его после входа");
                        this.Close();
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
