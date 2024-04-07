using AMOGUSIK.Entities;
using AMOGUSIK.ViewModels;
using System.Windows;


namespace AMOGUSIK
{
    /// <summary>
    /// Логика взаимодействия для Specialists.xaml
    /// </summary>
    public partial class Specialists : Window
    {
            public Specialists()
            {
                InitializeComponent();
                DataContext = new MainViewModel();
            }

        public List<ServiceOrders> GetOrders()
        {
            using (var context = new AudiCenterusContext())
            {
                return context.ServiceOrders.ToList();
            }
        }

        private void Button_ClickVK(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickTG(object sender, RoutedEventArgs e)
        {

        }
    }
}
