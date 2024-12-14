
using System.Configuration;
using System.Windows;
using DAL;
using ShoppingApp;
using BLL;
using DAL.Models;

namespace ShoppingApp
{
    public partial class App : Application
    {
        public static IDataService<Entity> DataService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Чтение конфигурации из appsettings
            string dataSourceType = ConfigurationManager.AppSettings["DataSourceType"] ?? "Database";
            string csvFilePathShops = ConfigurationManager.AppSettings["CsvFilePathShops"] ?? "Shops.csv";
            string csvFilePathProducts = ConfigurationManager.AppSettings["CsvFilePathProducts"] ?? "Products.csv";

            // Установка зависимости на основе типа источника данных
            if (dataSourceType == "Database")
            {
                var dbContext = new ApplicationDbContext();
                DataService = new DatabaseDataService<Entity>(dbContext);  // Используется для работы с БД

            }
            else if (dataSourceType == "Csv")
            {
                DataService = new CsvDataService<Entity>(csvFilePathShops, csvFilePathProducts);  // Используется для работы с CSV
            }
            else
            {
                MessageBox.Show("Unsupported data source type.");
                Shutdown();
            }

            // Запуск основного окна
            //var mainWindow = new MainWindow();
            //mainWindow.Show();
        }
    }
}
