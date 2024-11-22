using ShoppingApp.Models;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoppingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApplicationContext applicationContext = new ApplicationContext();
            applicationContext.Shops.Add(new Shop(1, "Дикси", "Ладожская 12"));
            Product product = new Product(3432, "Ipad", 1, 5, 334.5);
            applicationContext.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}