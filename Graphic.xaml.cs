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

            // Сложности лабораторных работ (1-7 и 9-10) в терминах Big O
            string[] labMaxComplexities = { "O(1)", "O(1)", "O(2^n)", "O(n)", "O(n log n)", "O(n)", "O(2^n)", "O(n)", "O(1)" };
            string[] labAvgComplexities = { "O(1)", "O(1)", "O(n log n)", "O(log n)", "O(n)", "O(log n)", "O(n log n)", "O(log n)", "O(1)" };

            // Преобразование сложности в числовые значения для упрощения отображения
            Dictionary<string, double> complexityMap = new Dictionary<string, double>
            {
                { "O(1)", 1 },
                { "O(log n)", 2 },
                { "O(n)", 3 },
                { "O(n log n)", 4 },
                { "O(n^2)", 5 },
                { "O(n^3)", 6 },
                { "O(2^n)", 7 },
                { "O(n!)", 8 }
            };

            // Преобразование строк в числовые значения
            double[] labMaxComplexitiesNum = labMaxComplexities.Select(c => complexityMap[c]).ToArray();
            double[] labAvgComplexitiesNum = labAvgComplexities.Select(c => complexityMap[c]).ToArray();

            // Настройка данных для графика
            SeriesCollection series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Максимальная сложность",
                    Values = new ChartValues<double>(labMaxComplexitiesNum),
                    ColumnPadding = 5,
                    MaxColumnWidth = 50,
                    Fill = System.Windows.Media.Brushes.Red
                },
                new ColumnSeries
                {
                    Title = "Средняя сложность",
                    Values = new ChartValues<double>(labAvgComplexitiesNum),
                    ColumnPadding = 5,
                    MaxColumnWidth = 50,
                    Fill = System.Windows.Media.Brushes.Blue
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
                Title = "Сложность (Big O)",
                Labels = complexityMap.Keys.ToArray(),
                MinValue = 0,
                MaxValue = complexityMap.Values.Max(),
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
