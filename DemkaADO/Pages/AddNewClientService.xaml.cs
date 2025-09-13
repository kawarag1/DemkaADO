using DemkaADO.Models;
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
    /// Логика взаимодействия для CloseClientServices.xaml
    /// </summary>
    public partial class AddNewClientServices : Page
    {
        public static Service service_;
        public AddNewClientServices(Service service)
        {
            InitializeComponent();
            service_ = service;
            DurationText.Text = service.DurationInSeconds.ToString() + " минут";
            TitleText.Text = service.Title.ToString();
            InitializeClients();
        }

        public void InitializeClients()
        {
            var db = Helper.GetContext();
            ClientsBox.ItemsSource = db.Client.ToList();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.GetContext();
            ClientService newServiceClient = new ClientService();
            newServiceClient.ServiceID = service_.ID;
            var client = ClientsBox.SelectedItem as Client;
            newServiceClient.ClientID = client.ID;
            newServiceClient.StartTime = GetSqlDateTime();
            newServiceClient.Comment = "";
            db.ClientService.Add(newServiceClient);
            db.SaveChanges();
            MessageBox.Show("Успешно!");
            NavigationService.GoBack();
        }

        public DateTime GetSqlDateTime()
        {
            if (datePicker.SelectedDate == null || string.IsNullOrEmpty(timeTextBox.Text))
                throw new InvalidOperationException("Дата и время не выбраны");

            try
            {
                TimeSpan time = TimeSpan.Parse(timeTextBox.Text);

                DateTime selectedDate = datePicker.SelectedDate.Value;
                DateTime result = new DateTime(
                    selectedDate.Year,
                    selectedDate.Month,
                    selectedDate.Day,
                    time.Hours,
                    time.Minutes,
                    time.Seconds);

                return result;
            }
            catch (FormatException)
            {
                throw new FormatException("Неверный формат времени. Используйте HH:mm");
            }
        }
    }
}
