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
    /// Логика взаимодействия для Reviews.xaml
    /// </summary>
    public partial class Reviews : UserControl
    {
        public List<ReviewEntity> reviews = new List<ReviewEntity>();
        public Patterns.UnitOfWork UnitOfWork = new Patterns.UnitOfWork();
        public Reviews()
        {
            InitializeComponent();
            ReviewsList.ItemsSource = null;
            if(UnitOfWork.ReviewRepo.LoadReviews() != null)
            {
                ReviewsList.ItemsSource = UnitOfWork.ReviewRepo.LoadReviews();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить отзывы, попробуйте позже или обратитеть в тех. поддержку");
            }
            
            
            
        }

        private void SendReview_Click(object sender, RoutedEventArgs e)
        {
            if(ReviewText.Text.Length<10)
            {
                MessageBox.Show("Недостаточная длина комментария");
            }
            else if(ReviewText.Text.Length>100)
            {
                MessageBox.Show("Слишком большой комментарий");
            }
            else
            {
                UnitOfWork.ReviewRepo.Create(MainWindow.user.Id, ReviewText.Text);
                MainWindow.MyForm.FieldForTemplate.Children.Clear();
                MainWindow.MyForm.FieldForTemplate.Children.Add(new Templates.Reviews());
            }
            
        }

        
    }
}
