using ShoppingApp.BLL;
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
            applicationContext.SaveChanges();

            AddProductButton.Click += AddProductButtonClick;
            AddShopButton.Click += AddShopButtonClick;
            SaveNewShop.Click += AddingShop;
            SaveNewProduct.Click += AddingProduct;
            SupplyButton.Click += SupplyAction;

            void AddShopButtonClick(object sender, RoutedEventArgs e)
            {
                ShopResult.Text = "";
                AddingProductBlock.Visibility = Visibility.Collapsed;
                AddingShopBlock.Visibility = Visibility.Visible;
            }
            
            void AddProductButtonClick(object sender, RoutedEventArgs e)
            {
                ProductResult.Text = "";
                AddingShopBlock.Visibility = Visibility.Collapsed;
                AddingProductBlock.Visibility = Visibility.Visible;
                
                ProductShopId.ItemsSource = applicationContext.Shops
                    .Select(s => s.Id + " " + "(" + s.Name + ")")
                    .ToList();
            }

            void AddingShop(object sender, RoutedEventArgs e)
            {

                string _shopName = AddingShopName.Text;
                string _shopAddress = AddingShopAdress.Text;

                try
                {
                    shopFactory.AddToDatabase(shopFactory.CreateEntity(_shopName, _shopAddress), applicationContext);
                    ProductShopId.ItemsSource = applicationContext.Shops
                        .Select(s => s.Id + " " + "(" + s.Name + ")")
                        .ToList();
                    ShopResult.Text = "Данные успешно сохранены";
                }
                catch
                {
                    throw new Exception("Ошибка добавления в БД");
                }
            }
            
            void AddingProduct(object sender, RoutedEventArgs e)
            {

                string _productName = ProductName.Text;
                int _productShopId = int.Parse(ProductShopId.Text[0].ToString());
                int _productAmount = int.Parse(ProductAmount.Text);
                double _productPrice = Double.Parse(ProductPrice.Text);

                try 
                {
                    productFactory.AddToDatabase(productFactory.CreateEntity(_productName, _productShopId, _productAmount, _productPrice), applicationContext);
                    ProductResult.Text = "Данные успешно сохранены";
                }
                catch 
                {
                    throw new Exception("Ошибка добавления в БД");
                }
            }

            void SupplyAction (object sender, RoutedEventArgs e)
            {
                SupplyResult.Text = "";
                SupplyBlock.Visibility = Visibility.Visible;
                FindCheapestSetBlock.Visibility = Visibility.Collapsed;
                FindCheapestBlock.Visibility = Visibility.Collapsed;
                SupplyShopId.ItemsSource = applicationContext.Shops
                        .Select(s => s.Id + " " + "(" + s.Name + ")")
                        .ToList();
                SupplyShopId.DropDownClosed += SetProducts;

                void SetProducts(object sender, EventArgs e)
                {
                    var filteredProducts = applicationContext.Products
                        .Where(p => p.ShopId == int.Parse(SupplyShopId.Text[0].ToString()))
                        .Select(p => p.Id + " " + "(" + p.Name + " " + p.Price +")")
                        .ToList();

                    SupplyProduct.ItemsSource = filteredProducts;
                }

                SupplySave.Click += SupplySave_Click;

                void SupplySave_Click(object sender, RoutedEventArgs e)
                {
                    int _amount = int.Parse(SupplyAmount.Text);
                    double _price = (SupplyPrice.Text != "") ? double.Parse(SupplyPrice.Text) : 0.0;
                    int _productId = int.Parse(SupplyProduct.Text[0].ToString());

                    Supplier supplier = new Supplier();
                    try
                    {
                        supplier.SupplyProduct(_productId, _amount, _price);
                        SupplyResult.Text = "Товар успешно добавлен в магазин";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ошибка обновления базы данных");
                    }
                }
            }
        }
    }
}