using ShoppingApp.BLL.Factories;
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
            ApplicationDbContext applicationContext = new ApplicationDbContext();
            ShopFactory shopFactory = new ShopFactory();
            ProductFactory productFactory = new ProductFactory();
            shopFactory.AddToDatabase(shopFactory.CreateEntity("Дикси", "Ладожская 14"), applicationContext);
            shopFactory.AddToDatabase(shopFactory.CreateEntity("Магнит", "Ладожская 14"), applicationContext);
            productFactory.AddToDatabase(productFactory.CreateEntity("Ножницы \'Кусь\'", 2, 15, 215.45), applicationContext);
            applicationContext.SaveChanges();
        }
    }
}