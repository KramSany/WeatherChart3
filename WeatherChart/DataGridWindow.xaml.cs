using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace WeatherChart
{
    /// <summary>
    /// Логика взаимодействия для DataGridWindow.xaml
    /// </summary>
    public partial class DataGridWindow : Window
    {
        private Dictionary<string, double[]> windData;
        public string CurrentDay { get; set; }
        public DataGridWindow(Dictionary<string, double[]> windData, bool isAdmin)
        {
            InitializeComponent();
            this.windData = windData;
            CurrentDay = MainWindow.CurrentDays;
            dataGrid.CellEditEnding += DataGrid_CellEditEnding;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = GetHourlyWeatherData(windData);
            dataGrid.IsReadOnly = !isAdmin;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedItem = (HourlyWeather)e.Row.Item;
                var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);
                UpdateWindData(editedItem.Hour, newValue);
            }
        }

        private void UpdateWindData(int hour, double newValue)
        {
            if (windData.TryGetValue(CurrentDay, out var speeds))
            {
                speeds[hour] = newValue;
            }
        }

        private ObservableCollection<HourlyWeather> GetHourlyWeatherData(Dictionary<string, double[]> windData)
        {
            var hourlyWeatherData = new ObservableCollection<HourlyWeather>();

            if (windData.TryGetValue(CurrentDay, out var speeds))
            {
                for (int hour = 0; hour < speeds.Length; hour++)
                {
                    hourlyWeatherData.Add(new HourlyWeather { Hour = hour, WindSpeed = speeds[hour] });
                }
            }

            return hourlyWeatherData;
        }

    }
}
