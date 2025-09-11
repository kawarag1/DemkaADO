using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using DemkaADO.Models;

namespace DemkaADO.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewServicePage.xaml
    /// </summary>
    public partial class AddNewServicePage : Page
    {
        public static Service service = new Service();
        public AddNewServicePage()
        {
            InitializeComponent();
        }

        private void ImageOfService_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|All files (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                LoadImage(selectedFilePath);

                service.MainImagePath = selectedFilePath;
            }
        }

        private void LoadImage(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                ImageOfService.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.GetContext();
            var services = db.Service.Select(s => s.Title).ToList();
            for (int i = 0; i < services.Count(); i++)
            {
                if (services[i] == NameOfService.Text)
                {
                    MessageBox.Show("Услуга с таким названием уже существует");
                    break;
                }
            }
            service.Title = NameOfService.Text;
            service.Cost = Convert.ToDecimal(CostOfService.Text);
            service.Description = Description.Text;
            if (Convert.ToInt32(Duration.Text) > -1 || Convert.ToInt32(Duration.Text) < 240)
            {
                service.DurationInSeconds = Convert.ToInt32(Duration.Text) * 60;
            }
            else
            {
                MessageBox.Show("Процедура не может идти столько времени!");
                
            }
            service.Discount = Convert.ToDouble(Convert.ToInt32(Discount.Text) / 100);
            db.SaveChanges();
            MessageBox.Show("Успешно!");
            NavigationService.GoBack();
        }
    }
}
