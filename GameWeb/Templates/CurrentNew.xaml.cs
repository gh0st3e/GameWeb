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
    /// Логика взаимодействия для CurrentNew.xaml
    /// </summary>
    public partial class CurrentNew : UserControl
    {
        public CurrentNew(NewsEntity news)
        {
            InitializeComponent();
            try
            {
                NewsImage.Source = new BitmapImage(new Uri(news.Image));
            }
            catch
            {

            }
            NewsName.Text = news.Name;
            NewsDescr.Text = news.Descr;
        }
    }
}
