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
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace AMOGUSIK
{
    /// <summary>
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : Window
    {
        public Graphic()
        {
            InitializeComponent();

            // Сложности лабораторных работ (1-7 и 9-10)
            int[] labComplexities = { 1, 1, 9, 3, 5, 4, 9, 4, 1 };

            // Настройка данных для графика
            SeriesCollection series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Сложность лабы",
                    Values = new ChartValues<int>(labComplexities),
                    ColumnPadding = 5,
                    MaxColumnWidth = 50
                }
            };

            chart.Series = series;

            // Настройка осей
            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis
            {
                Title = "Лабораторные работы",
                Labels = new[] { "1", "2", "3", "4", "5", "6", "7", "9", "10" }
            });

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                Title = "Сложность",
                MinValue = 0,
                MaxValue = 10,
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1,
                    IsEnabled = false
                }
            });

            // Настройка размеров и стилей графика
            chart.LegendLocation = LegendLocation.Top;
            chart.Padding = new Thickness(10);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Specialists specialists = new Specialists();
            specialists.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickVK(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickTG(object sender, RoutedEventArgs e)
        {

        }

    }
}
