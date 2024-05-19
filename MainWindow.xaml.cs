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
using System.Diagnostics;


namespace AMOGUSIK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

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
                var adminRole = Roles.Admin;

                var customer = dbContext.Customers
                    .FirstOrDefault(c => c.Username == enteredUsername && c.Password == enteredPassword);

                if (customer != null)
                {
                    //MessageBox.Show("Здравствуйте!");

                    if (customer.RoleID == adminRole.RoleID)
                    {
                        Specialists adminWindow = new Specialists();
                        adminWindow.Show();
                    }
                    else
                    {
                        MainForm userWindow = new MainForm();
                        userWindow.Show();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
        }

        private void RegClk(object sender, RoutedEventArgs e)
        {
            Registration Reg = new Registration();
            Reg.Show();
            this.Close();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // При наведении мыши, показываем TextBox и копируем пароль из PasswordBox
            txtPass.Text = txtPassINV.Password;
            txtPass.Visibility = Visibility.Visible;
            txtPassINV.Visibility = Visibility.Collapsed;
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // При уходе мыши, возвращаемся к исходному состоянию
            txtPass.Visibility = Visibility.Collapsed;
            txtPassINV.Visibility = Visibility.Visible;
        }

    }

}


