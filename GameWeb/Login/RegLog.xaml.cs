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
using System.Windows.Shapes;

namespace GameWeb.Login
{
    /// <summary>
    /// Логика взаимодействия для RegLog.xaml
    /// </summary>
    /// 
    
    public partial class RegLog : Window
    {
        public string DefaultImage = "https://sun9-13.userapi.com/s/v1/ig2/04e4zWFRl5DMeRI_1n29tFA2PiWKi-3sjPfGUJ6w03UPz6P6P-5WWReZ_ujpWzsm8wQvWqHkuYMbPO6xsLwP1Fbw.jpg?size=1920x1920&quality=96&type=album";
        public bool IsLogin = true;
        public LogIn logIn = new LogIn();
        public LogUp logUp = new LogUp();
        Patterns.UnitOfWork UnitOfWork = new Patterns.UnitOfWork();
        public RegLog()
        {
            InitializeComponent();
            
            FieldForRegLog.Children.Clear();
            FieldForRegLog.Children.Add(logIn);
            
        }

        private void NewAccount_Click(object sender, RoutedEventArgs e)
        {
            if(IsLogin)
            {
                FieldForRegLog.Children.Clear();
                FieldForRegLog.Children.Add(logUp);
                LogInUp.Text = "Создать новый аккаунт";
                LogInBtn.Content = "Создать";
                NewAccountBtn.Content = "У меня есть аккаунт";
                IsLogin = false;
            }
            else
            {
                FieldForRegLog.Children.Clear();
                FieldForRegLog.Children.Add(logIn);
                LogInUp.Text = "Вход в аккаунт GameWeb";
                LogInBtn.Content = "Войти";
                NewAccountBtn.Content = "У меня нет аккаунта";
                IsLogin = true;
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IsLogin)
            {             
                try
                {
                    
                    if(UnitOfWork.UserRepo.GetUser(logIn.Login.Text, logIn.Password.Password)==null)
                    {
                        MessageBox.Show("Неверные данные для входа");
                        return;
                    }
                    MainWindow.user = UnitOfWork.UserRepo.GetUser(logIn.Login.Text, logIn.Password.Password);
                    MainWindow.MyForm.Visibility = Visibility.Visible;
                    MainWindow.MyForm.OnUserChange();
                    this.Close();                             
                }
                catch
                {
                    MessageBox.Show("Проблемы со входом, обратитесь в тех поддержку");
                    return;
                }

            }
            else
            {
                string NewLogin = logUp.Login.Text;
                string NewPassword = logUp.Password.Password;
                if (NewLogin.Length < 3 || NewPassword.Length < 3)
                {
                    MessageBox.Show("Недостаточная длина логина или пароля");
                    return;
                }
                if(NewLogin.Length>20)
                {
                    MessageBox.Show("Слишком длинный логин");
                    return;
                }
                if(NewPassword.Length>30)
                {
                    MessageBox.Show("Слишком длинный пароль");
                    return;
                }
               
                string NewName = logUp.Name.Text;
                if(NewName.Length<3)
                {
                    MessageBox.Show("Недостаточная длина имени");
                    return;
                }
                if(NewName.Length>20)
                {
                    MessageBox.Show("Слишком длинное имя");
                    return;
                }
                if(UnitOfWork.UserRepo.GetID()==-1)
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                    return;
                }
                int NewID = UnitOfWork.UserRepo.GetID()+1;
                bool Admin = false;
                string datefrom = DateTime.Now.ToShortDateString();
                if (!UnitOfWork.UserRepo.CheckUser(NewLogin,NewName))
                {
                    MessageBox.Show("Пользователь с таким логином или паролем уже существует");  
                    return;
                }
                else
                {
                    try
                    {
                        UnitOfWork.UserRepo.Create(NewID, NewLogin, NewPassword, NewName, Admin, datefrom, DefaultImage);
                        MessageBox.Show("Аккаунт был создан");
                        FieldForRegLog.Children.Clear();
                        FieldForRegLog.Children.Add(logIn);
                        LogInUp.Text = "Вход в аккаунт GameWeb";
                        LogInBtn.Content = "Войти";
                        NewAccountBtn.Content = "У меня нет аккаунта";
                        logIn.Login.Text = NewLogin;
                        IsLogin = true;
                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка, обратитесь в тех. поддержку");
                    }
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Templates.ForgotPassword forgotPassword = new Templates.ForgotPassword();
            forgotPassword.Show();
        }
    }
}
