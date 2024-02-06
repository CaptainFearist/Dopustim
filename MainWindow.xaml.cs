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

        public MainWindow()
        {
            InitializeComponent();
         
        }

        private void enterClk(object sender, RoutedEventArgs e)
        {

                string Login = txtLog.Text;
                string Password = txtPassINV.Password;


            using (var dbContext = new CenterAudiContext())
            {
                var user = dbContext.Customers.FirstOrDefault(c => c.Username == Login && c.Password == Password);

                if (user != null)
                {
                    if (dbContext.Roles.FirstOrDefault(r => r.RoleName == "Администратор").RoleID == user.RoleID)
                    {
                        Specialists adminWindow = new Specialists();
                        adminWindow.Show();
                        this.Close();
                    }
                    else if (dbContext.Roles.FirstOrDefault(r => r.RoleName == "Клиент").RoleID == user.RoleID)
                    {
                        MainForm userWindow = new MainForm();
                        userWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            //MainForm mf = new MainForm();
            //mf.Show();
            //this.Close();
        }

        private void RegClk(object sender, RoutedEventArgs e)
        {
            Registration Reg = new Registration();
            Reg.Show();
            this.Close();
        }
    }
}