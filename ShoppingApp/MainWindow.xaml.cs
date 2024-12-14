using BLL;
using DAL;
using System.Configuration;
using System.IO;
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

            ApplicationDbContext applicationContext = default;

            if (App.DataService.GetType().IsGenericType && App.DataService.GetType().GetGenericTypeDefinition() == typeof(DatabaseDataService<>))
            {
                applicationContext = new ApplicationDbContext();
                applicationContext.SaveChanges();
            }

            AddProductButton.Click += AddProductButtonClick;
            AddShopButton.Click += AddShopButtonClick;
            SaveNewShop.Click += AddingShop;
            SaveNewProduct.Click += AddingProduct;
            SupplyButton.Click += SupplyAction;
            CheaperButton.Click += CheaperAction;
            PossibleButton.Click += PotentialProductsAction;
            BuyButton.Click += BuyProducts;

            List<string> Findshops()
            {
                if (applicationContext != null)
                {
                    return applicationContext.Shops
                        .Select(s => s.Id + " " + "(" + s.Name + ", " + s.Address + ")")
                        .ToList();
                }
                else
                {
                    var shops = new List<string>();

                    // Замените на путь к вашему CSV-файлу
                    string filePath = ConfigurationManager.AppSettings["CsvFilePathToShop"];

                    try
                    {
                        using (var reader = new StreamReader(filePath))
                        {
                            string line;
                            // Пропускаем заголовок, если он есть, или начинаем сразу с первой строки
                            bool isFirstLine = true;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (isFirstLine)
                                {
                                    // Пропускаем первую строку с заголовками, если нужно
                                    isFirstLine = false;
                                    continue;
                                }

                                // Разделяем строку по запятой
                                string[] values = line.Split(',');

                                if (values.Length >= 3) // Если строка корректная (по количеству колонок)
                                {
                                    int id;
                                    string name = values[1].Trim();
                                    string address = values[2].Trim();

                                    // Проверка на корректность данных, например, проверка на ID
                                    if (int.TryParse(values[0].Trim(), out id))
                                    {
                                        // Формируем строку с данными о магазине
                                        shops.Add($"{id} ({name}, {address})");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок чтения файла CSV
                        MessageBox.Show($"Ошибка при чтении CSV файла: {ex.Message}");
                    }

                    return shops;
                }
            }

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

                ProductShopId.ItemsSource = Findshops();
            }

            void AddingShop(object sender, RoutedEventArgs e)
            {

                string _shopName = AddingShopName.Text;
                string _shopAddress = AddingShopAdress.Text;

                try
                {
                    App.DataService.AddToDatabase(App.DataService.CreateEntity("shop", _shopName, _shopAddress), applicationContext);
                    ProductShopId.ItemsSource = Findshops();
                    ShopResult.Text = "Данные успешно сохранены";
                }
                catch
                {
                    throw new Exception("Ошибка добавления в БД");
                }
                applicationContext.SaveChanges();
            }

            void AddingProduct(object sender, RoutedEventArgs e)
            {

                string _productName = ProductName.Text;
                int _productShopId = int.Parse(ProductShopId.Text[0].ToString());
                int _productAmount = int.Parse(ProductAmount.Text);
                double _productPrice = Double.Parse(ProductPrice.Text);

                try 
                {
                    App.DataService.AddToDatabase(App.DataService.CreateEntity("product", _productName, _productShopId, _productAmount, _productPrice), applicationContext);
                    ProductResult.Text = "Данные успешно сохранены";
                }
                catch 
                {
                    throw new Exception("Ошибка добавления в БД");
                }

                applicationContext.SaveChanges();
            }

            void SupplyAction (object sender, RoutedEventArgs e)
            {
                SupplyResult.Text = "";
                SupplyBlock.Visibility = Visibility.Visible;
                FindCheapestSetBlock.Visibility = Visibility.Collapsed;
                FindCheapestBlock.Visibility = Visibility.Collapsed;
                PotentialPurchaseBlock.Visibility = Visibility.Collapsed;
                PurchaseBlock.Visibility = Visibility.Collapsed;

                SupplyShopId.ItemsSource = Findshops();
                SupplyShopId.DropDownClosed += SetProducts;

                void SetProducts(object sender, EventArgs e)
                {

                    var filteredProducts = applicationContext.Products
                        .Where(p => p.ShopId == int.Parse(SupplyShopId.Text[0].ToString()))
                        .Select(p => p.Id + " " + "(" + p.Name + " " + p.Price + ")")
                        .ToList();
                    SupplyProduct.ItemsSource = filteredProducts;
                }

                SupplySave.Click += SupplySave_Click;

                void SupplySave_Click(object sender, RoutedEventArgs e)
                {
                    string text = SupplyProduct.Text;
                    int spaceIndex = text.IndexOf(' ');
                    int _productId = int.Parse(text.Substring(0, spaceIndex));
                    int _amount = int.Parse(SupplyAmount.Text);
                    double _price = (SupplyPrice.Text != "") ? double.Parse(SupplyPrice.Text) : 0.0;
                    //int _productId = int.Parse(SupplyProduct.Text[0].ToString());
                    try
                    {
                        App.DataService.SupplyProduct(_productId, _amount, _price);
                        SupplyResult.Text = "Товар успешно добавлен в магазин";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ошибка обновления базы данных");
                    }
                }
            }

            void CheaperAction(object sender, RoutedEventArgs e)
            {
                SupplyBlock.Visibility = Visibility.Collapsed;
                FindCheapestBlock.Visibility = Visibility.Visible;
                PotentialPurchaseBlock.Visibility = Visibility.Collapsed;
                PurchaseBlock.Visibility = Visibility.Collapsed;

                CheaperProductButton.Click += CheaperProductAction;

                void CheaperProductAction(object sender, RoutedEventArgs e)
                {
                    CheaperProductResult.Text = "";
                    string _prodName = CheaperProduct.Text;
                    CheaperProductResult.Text = App.DataService.FindTheCheapestShop(_prodName);
                }
            }

            void PotentialProductsAction(object sender, RoutedEventArgs e)
            {
                PotentialAvailableProductsList.Text = "";
                PotentialAvailableAmount.Text = "";
                PotentialPurchaseBlock.Visibility = Visibility.Visible;
                SupplyBlock.Visibility = Visibility.Collapsed;
                FindCheapestSetBlock.Visibility = Visibility.Collapsed;
                FindCheapestBlock.Visibility = Visibility.Collapsed;
                PurchaseBlock.Visibility = Visibility.Collapsed;
                PotentialCalculateButton.Click += PotentialCalculateButton_Click;

                PotentialShopSelector.ItemsSource = Findshops();

                void PotentialCalculateButton_Click(object sender, RoutedEventArgs e)
                {
                    string text = PotentialAvailableAmount.Text;
                    int spaceIndex = text.IndexOf(' ');
                    int shopId = int.Parse(text.Substring(0, spaceIndex));
                    PotentialAvailableProductsList.Text = "";
                    double _anount = double.Parse(PotentialAvailableAmount.Text);
                    //int shopId = int.Parse(PotentialShopSelector.Text[0].ToString());

                    foreach (string item in App.DataService.FindProductsToBuy(shopId, _anount))
                    {
                        PotentialAvailableProductsList.Text += item + "\n";
                    }
                }

            }
            
            void BuyProducts(object sender, RoutedEventArgs e)
            {
                PurchaseQuantity.Text = "";
                PurchaseResult.Text = "";
                PotentialAvailableProductsList.Text = "";
                PotentialAvailableAmount.Text = "";
                PurchaseBlock.Visibility = Visibility.Visible;
                PotentialPurchaseBlock.Visibility = Visibility.Collapsed;
                SupplyBlock.Visibility = Visibility.Collapsed;
                FindCheapestSetBlock.Visibility = Visibility.Collapsed;
                FindCheapestBlock.Visibility = Visibility.Collapsed;
                PurchasePriceButton.Click += PurchasePriceButton_click;
                PurchaseButton.Click += PurchaseButton_click;

                PurchaseShopSelector.ItemsSource = Findshops();
                PurchaseShopSelector.DropDownClosed += SetPurchaseProducts;

                void SetPurchaseProducts (object sender, EventArgs e)
                {
                    PurchaseProductSelector.ItemsSource = applicationContext.Products
                            .Where(p => p.ShopId == int.Parse(PurchaseShopSelector.Text[0].ToString()))
                            .Select(p => p.Id + " " + "(" + p.Name + " " + p.Price + ")")
                            .ToList();
                }

                void PurchaseButton_click(object sender, RoutedEventArgs e)
                {
                    string text = PurchaseProductSelector.Text;
                    int spaceIndex = text.IndexOf(' ');
                    int _prodId = int.Parse(text.Substring(0, spaceIndex));
                    PurchaseResult.Text = "";
                    int _amount = int.Parse(PurchaseQuantity.Text);
                    PurchaseResult.Text += App.DataService.BuyProducts(_prodId, _amount) + "\n";
                }

                void PurchasePriceButton_click (object sender, RoutedEventArgs e)
                {
                    string text = PurchaseProductSelector.Text;
                    int spaceIndex = text.IndexOf(' ');
                    int _prodId = int.Parse(text.Substring(0, spaceIndex));
                    PurchaseResult.Text = "";
                    int _amount = int.Parse(PurchaseQuantity.Text);
                    PurchaseResult.Text += App.DataService.GetTheFinalPrice(_prodId, _amount) + "\n";
                }
            }
        }
    }
}