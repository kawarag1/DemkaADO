using DemkaADO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Threading;

namespace DemkaADO.Pages
{
    /// <summary>
    /// Логика взаимодействия для CloseServices.xaml
    /// </summary>
    public partial class CloseServices : Page
    {
        public CloseServices()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
        }

        public class AppointmentInfo
        {
            public string ServiceName { get; set; }
            public string ClientFullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime StartTime { get; set; }
            public string TimeRemaining { get; set; }
            public bool IsLessThanHour { get; set; }
        }


        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadAppointments();
            StartAutoRefresh();
        }

        private void LoadAppointments()
        {
            try
            {
                var db = Helper.GetContext();
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                var appointments = db.ClientService
                    .Where(cs => DbFunctions.TruncateTime(cs.StartTime) == today ||
                                 DbFunctions.TruncateTime(cs.StartTime) == tomorrow)
                    .Include(cs => cs.Service)
                    .Include(cs => cs.Client)
                    .OrderBy(cs => cs.StartTime)
                    .ToList();

                var appointmentInfos = appointments.Select(cs => new AppointmentInfo
                {
                    ServiceName = cs.Service?.Title ?? "Неизвестная услуга",
                    ClientFullName = $"{cs.Client?.LastName} {cs.Client?.FirstName} {cs.Client?.Patronymic}".Trim(),
                    Email = cs.Client?.Email ?? "",
                    Phone = cs.Client?.Phone ?? "",
                    StartTime = cs.StartTime,
                    TimeRemaining = FormatTimeRemaining(cs.StartTime - DateTime.Now),
                    IsLessThanHour = (cs.StartTime - DateTime.Now).TotalHours < 1 &&
                                   (cs.StartTime - DateTime.Now).TotalSeconds > 0
                })
                .Where(a => a.StartTime > DateTime.Now) // Только будущие записи
                .ToList();

                appointmentsListView.ItemsSource = appointmentInfos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FormatTimeRemaining(TimeSpan timeRemaining)
        {
            if (timeRemaining.TotalSeconds <= 0)
                return "Запись прошла";

            if (timeRemaining.TotalDays >= 1)
            {
                return $"{(int)timeRemaining.TotalDays} дн. {timeRemaining.Hours} час.";
            }

            if (timeRemaining.TotalHours >= 1)
            {
                int hours = (int)timeRemaining.TotalHours;
                int minutes = timeRemaining.Minutes;
                return $"{hours} час. {minutes} мин.";
            }

            return $"{timeRemaining.Minutes} мин.";
        }


        private void StartAutoRefresh()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += (s, e) => LoadAppointments();
            timer.Start();
        }
    }
}
