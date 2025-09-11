using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для ChangeServicePage.xaml
    /// </summary>
    public partial class ChangeServicePage : Page
    {
        public static Service service;
        public static string NewImagePath;
        public ChangeServicePage(Service service_)
        {
            InitializeComponent();
            service = service_;
            Discount.Text = Convert.ToString(service_.Discount * 100);
            Description.Text = service_.Description.ToString();
            Duration.Text = Convert.ToString(service_.DurationInSeconds / 60);
            CostOfService.Text = service_.Cost.ToString();
            NameOfService.Text = service_.Title.ToString();
            ImageOfService.DataContext = service_;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
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

                NewImagePath = selectedFilePath;
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

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.GetContext();
            service.Title = NameOfService.Text;
            service.Cost = Convert.ToDecimal(CostOfService.Text);
            service.Description = Description.Text;
            service.DurationInSeconds = Convert.ToInt32(Duration.Text) * 60;
            service.Discount = Convert.ToDouble(Convert.ToInt32(Discount.Text) / 100);
            if (NewImagePath != null)
            {
                service.MainImagePath = NewImagePath;
            }
            db.SaveChanges();
            MessageBox.Show("Успешно!");
            NavigationService.GoBack();
        }
    }
}
