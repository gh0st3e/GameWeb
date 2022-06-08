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
    /// Логика взаимодействия для ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        public Login.User Friend;
        public List<Chat> History = new List<Chat>();
        public int CurrentID;
        public ChatControl(Login.User user)
        {
            InitializeComponent();
            Friend = user;
            LoadChat();
        }
        public int GetId()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT id FROM chat ORDER BY id", connection);
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
                return CurrentID;
            }
            catch
            {
                MessageBox.Show("Возникли проблемы с работой приложения, попробуйте позже или обратитесь в тех. поддержку");
                return -1;
            }
        }
        public void LoadChat()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand($"SELECT chat.id,chat.userid,user.avatarimg,chat.text FROM chat INNER JOIN user ON chat.userid=user.id WHERE (chat.userid={MainWindow.user.Id} AND chat.friendid={Friend.Id}) OR (chat.userid={Friend.Id} AND chat.friendid={MainWindow.user.Id}) ORDER BY id ASC", connection);
                    MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            int id = int.Parse(reader.GetValue(0).ToString());
                            int userid = int.Parse(reader.GetValue(1).ToString());
                            string avatarimg = reader.GetValue(2).ToString();
                            string text = reader.GetValue(3).ToString();
                            Chat chat = new Chat(id, userid, avatarimg, text);
                            History.Add(chat);

                        }
                    }
                    connection.Close();
                }
                ChatList.ItemsSource = null;
                ChatList.ItemsSource = History;
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки чата или у вас возникли неполадки с интернетом");
            }
        }

        private void SendMes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetId();
                using (MySqlConnection connection = new MySqlConnection(DataBase.Connection.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand($"INSERT INTO CHAT (id,userid,friendid,text) VALUES ({++CurrentID},{MainWindow.user.Id},{Friend.Id},'{NewMessage.Text}')", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new ChatControl(Friend));
            }
            catch
            {
                MessageBox.Show("Не удалось отправить сообщение пользователю либо у вас возникли неполадки с интернетом");
            }
        }
    }
}
