using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeatherChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string CurrentDays
        {
            get; set;
        }
        private List<HourlyWeather> hourlyWeatherData = new List<HourlyWeather>();
        private Dictionary<string, double[]> windData;
        private bool isAdmin = false;
        Weather weather = new();
        List<string> daysofWeek = new List<string>() { "today", "tomorrow", "afterTomorrow", "FourthDay" };
        private bool _isPaused = true;
        Dictionary<double,double> dictCoords = new Dictionary<double,double>();
        public int i = 0;
        public int j = 0;
        public MainWindow()
        {
            weather.GetWindForthDays();
            windData = weather.CollectionWindAndDays;
            InitializeComponent();
            AddPointsAsync();
        }

        private void RedrawGraph()
        {
            polyline.Points.Clear();

            if (windData.TryGetValue("today", out var speeds))
            {
                for (int k = 0; k < speeds.Length; k++)
                {
                    double x = 50 + k * 31;
                    double y = 345 - speeds[k] * 30;
                    polyline.Points.Add(new Point(x, y));
                }
            }
            // Аналогично обновите значения для других дней
        }

        private async Task AddPointsAsync()
        {
            while (_isPaused)
            {
                if (j == 4)
                {
                    j = 0;
                }
                nameDaysOfWeek.Text = daysofWeek[j];
                
                double[] speeds = weather.DaysWeather(daysofWeek[j]);
                double x = 50 + i * 31;
                double y = 345 - speeds[i] * 30;
                dictCoords.Add(x, y);
                
                polyline.Points.Add(new Point(x, y));
                await Task.Delay(1000);
                i++;

                if (i == 24)
                {
                    i = 0;
                    polyline.Points.Clear();
                    j++;
                    dictCoords.Clear();
                    UpdateHourlyWeatherData();
                }
            }
        }

        private void freezeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                if (_isPaused == false)
                {
                    DrawAfterfreeze();
                    _isPaused = true;
                    AddPointsAsync();
                }
                else
                {
                    _isPaused = false;
                }
            }
            else
            {
                MessageBox.Show("Вы обычный юзер. Изменять данные может только администатор");
                
            }
            if (isAdmin && _isPaused == false)
            {
                CurrentDays = nameDaysOfWeek.Text;
                DataGridWindow dataGridWindow = new DataGridWindow(windData, isAdmin);
                dataGridWindow.CurrentDay = nameDaysOfWeek.Text;
                dataGridWindow.Show();
            }
        }

        private void DrawAfterfreeze()
        {
            i = dictCoords.Count;
            polyline.Points.Clear();
            foreach (var item in dictCoords)
            {
                polyline.Points.Add(new Point(item.Key, item.Value));
            }
        }

        public void SetAdminStatus(bool status)
        {
            isAdmin = status;
        }

        private void UpdateGraphWithData(double[] newSpeeds)
        {
            if (newSpeeds.Length == 24)
            {
                polyline.Points.Clear();

                for (int k = 0; k < newSpeeds.Length; k++)
                {
                    double x = 50 + k * 31;
                    double y = 345 - newSpeeds[k] * 30;
                    polyline.Points.Add(new Point(x, y));
                }
            }
            else
            {
                MessageBox.Show("Invalid data length.");
            }
        }

        private void UpdateHourlyWeatherData()
        {
            hourlyWeatherData.Clear();
            for (int hour = 0; hour < 24; hour++)
            {
                double[] speeds = weather.DaysWeather(daysofWeek[0]);
                hourlyWeatherData.Add(new HourlyWeather { Hour = hour, WindSpeed = speeds[hour] });
            }
        }
    }

}
