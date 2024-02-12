using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMOGUSIK.Entities;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;


namespace AMOGUSIK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class PasswordToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string password && !string.IsNullOrEmpty(password))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        private DbContextOptions<AudiCenterusContext> options;

        public MainWindow()
        {
            InitializeComponent();
         
        }

        private void enterClk(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtLog.Text;
            string enteredPassword = txtPassINV.Password;

            using (var dbContext = new AudiCenterusContext())
            {
                // Проверяем в таблице Customers
                var customer = dbContext.Customers
                    .FirstOrDefault(c => c.Username == enteredUsername && c.Password == enteredPassword);

                // Проверяем, найден ли пользователь в Customers
                if (customer != null)
                {
                    MessageBox.Show("Здравствуйте!");
                    MainForm userWindow = new MainForm();
                    userWindow.Show();
                    this.Close();
                }
                else
                {
                    // Неверные учетные данные
                    MessageBox.Show("Неверное имя пользователя или пароль.");
                }
            }

        }

        private void RegClk(object sender, RoutedEventArgs e)
        {
            Registration Reg = new Registration();
            Reg.Show();
            this.Close();
        }
    private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {

        }

    }

}


