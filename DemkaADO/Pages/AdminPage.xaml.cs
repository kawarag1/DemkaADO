using DemkaADO.Models;
using DemkaADO.Services;
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

namespace DemkaADO.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public static List<Service> services_;
        public AdminPage()
        {
            InitializeComponent();
            ServiceService service = new ServiceService();
            var services = service.GetServices();
            services_ = services;
            LVServices.ItemsSource = services;
            Counter.Content = $"Результатов поиска: {services.Count()}";
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                services_ = services_.OrderBy(s => s.FinalCost).ToList();
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
            else
            {
                services_ = services_.OrderByDescending(s => s.FinalCost).ToList();
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
        }

        private void DiscountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiscountBox.SelectedIndex == 0)
            {
                services_ = DiscountFilter(0, 0.05, services_);
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
            else if (DiscountBox.SelectedIndex == 1)
            {
                services_ = DiscountFilter(0.05, 0.15, services_);
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
            else if (DiscountBox.SelectedIndex == 2)
            {
                services_ = DiscountFilter(0.15, 0.30, services_);
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
            else if (DiscountBox.SelectedIndex == 3)
            {
                services_ = DiscountFilter(0.30, 0.70, services_);
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
            else
            {
                services_ = DiscountFilter(0.70, 1, services_);
                LVServices.ItemsSource = services_;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
            }
        }

        private List<Service> DiscountFilter(double min, double max, List<Service> services)
        {
            return services.Where(x => x.Discount >= min && x.Discount < max).OrderBy(x => x.Discount).ToList();
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Service selectedService)
            {
                NavigationService.Navigate(new ChangeServicePage(selectedService));
                ServiceService service = new ServiceService();
                var services = service.GetServices();
                services_ = services;
                Counter.Content = $"Результатов поиска: {services_.Count()}";
                LVServices.ItemsSource = services;
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = Search.Text;
            services_ = services_.Where(x => x.Description.Contains(query) || x.Title.Contains(query)).ToList();
            Counter.Content = $"Результатов поиска: {services_.Count()}";
            LVServices.ItemsSource = services_;
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            ServiceService service = new ServiceService();
            var services = service.GetServices();
            services_ = services;
            Counter.Content = $"Результатов поиска: {services_.Count()}";
            LVServices.ItemsSource = services;
        }
    }
}
