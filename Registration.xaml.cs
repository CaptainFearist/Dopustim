using AMOGUSIK.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace AMOGUSIK
{
    public partial class Registration : Window
    {
        private CenterAudiContext dbContext;

        public Registration()
        {
            InitializeComponent();
            dbContext = new CenterAudiContext();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow Ent = new MainWindow();
            Ent.Show();
            this.Close();
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            if (PSD2.Password != PSD3.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            Customers newUser = new Customers
            {
                FirstName = Name1.Text,
                LastName = Surname1.Text,
                Username = Login1.Text,
                Password = PSD2.Password,
                // Не указываем значение для CustomerID, если это поле автоматически увеличивается
                // RoleID устанавливаем, вызывая ваш метод GetRoleIdForClient()
                RoleID = GetRoleIdForClient()
            };

            dbContext.Customers.Add(newUser);
            dbContext.SaveChanges();

            MessageBox.Show("Регистрация успешна");
        }



        private int GetRoleIdForClient()
        {
            Roles clientRole = dbContext.Roles.FirstOrDefault(r => r.RoleName == "Клиент");

            if (clientRole == null)
            {
                clientRole = new Roles { RoleName = "Клиент" };
                dbContext.Roles.Add(clientRole);
                dbContext.SaveChanges();
            }

            return clientRole.RoleID;
        }
    }
}